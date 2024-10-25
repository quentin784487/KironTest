using Dapper;
using Kirontest.HttpClient.Contracts;
using KironTest.DAL.Contracts;
using KironTest.DataModel;
using KironTest.Repository.Contracts;
using KironTest.Shared.Exceptions;
using KironTest.Shared.ViewModels;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace KironTest.Repository
{
    public class HolidayRepository : IHolidayRepository
    {
        private readonly IHttpClientWrapper _httpClientWrapper;
        private readonly IDatabaseWrapper _databaseWrapper;
        private readonly IConfiguration _configuration;
        private readonly string? _maxConnections;

        public HolidayRepository(IHttpClientWrapper _httpClientWrapper, IDatabaseWrapper _databaseWrapper, IConfiguration _configuration)
        {
            this._httpClientWrapper = _httpClientWrapper;
            this._databaseWrapper = _databaseWrapper;
            this._configuration = _configuration;
        }

        public async Task<Dictionary<string, Region>> GetUKBankHolidays()
        {
            return await _httpClientWrapper.GetAsync<Dictionary<string, Region>>(_configuration["BankHolidaysUrl"]);
        }

        public async Task SetUKBankHolidays(List<Region> regions)
        {
            try
            {
                using (var connection = _databaseWrapper.GetConnection())
                {
                    using (var transaction = _databaseWrapper.BeginTransaction(connection))
                    {
                        try
                        {
                            foreach (var region in regions)
                            {
                                DynamicParameters parameters = new DynamicParameters();
                                parameters.Add("Name", region.Name);
                                parameters.Add("Division", region.Division);
                                parameters.Add("Id", dbType: DbType.Int32, direction: ParameterDirection.Output);

                                await _databaseWrapper.ExecuteStoredProcedureAsync("[dbo].[sp_SetRegion]", parameters, connection, transaction);
                                int regionId = parameters.Get<int>("Id");

                                foreach (var _event in region.Events)
                                {
                                    parameters = new DynamicParameters();
                                    parameters.Add("Description", _event.Title);
                                    parameters.Add("Id", dbType: DbType.Int32, direction: ParameterDirection.Output);

                                    await _databaseWrapper.ExecuteStoredProcedureAsync("[dbo].[sp_SetHolidayTitle]", parameters, connection, transaction);
                                    int holidayTitleId = parameters.Get<int>("Id");

                                    parameters = new DynamicParameters();
                                    parameters.Add("RegionId", regionId);
                                    parameters.Add("HolidayTitleId", holidayTitleId);
                                    parameters.Add("Date", _event.Date);
                                    parameters.Add("Notes", _event.Notes);
                                    parameters.Add("Bunting", _event.Bunting);

                                    await _databaseWrapper.ExecuteStoredProcedureAsync("[dbo].[sp_SetEvent]", parameters, connection, transaction);
                                }
                            }
                            
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw new DataAccessException("Error processing holiday events", ex);
                        }
                        finally
                        {
                            _databaseWrapper.ReleaseConnection(connection);
                        }
                    }
                }
            }
            catch (MaximumConnectionsException ex)
            {
                throw new DataAccessException($"A maximum of '{_maxConnections}' are allowed.", ex);
            }
        }

        public async Task<IEnumerable<RegionDTO>> GetAllRegions()
        {
            try
            {
                using (var connection = _databaseWrapper.GetConnection())
                {
                    try
                    {
                        return await _databaseWrapper.QueryListAsync<RegionDTO>("[dbo].[sp_GetAllRegions]", connection);
                    }
                    catch (Exception ex)
                    {
                        throw new DataAccessException("Error fetching all regions.", ex);
                    }
                    finally
                    {
                        _databaseWrapper.ReleaseConnection(connection);
                    }
                }
            }
            catch (MaximumConnectionsException ex)
            {
                throw new DataAccessException($"A maximum of '{_maxConnections}' are allowed.", ex);
            }
        }

        public async Task<IEnumerable<BankHolidayDTO>> GetAllBankHolidaysById(int RegionId)
        {
            try
            {
                using (var connection = _databaseWrapper.GetConnection())
                {
                    try
                    {
                        var parameters = new DynamicParameters();
                        parameters.Add("RegionId", RegionId);

                        return await _databaseWrapper.QueryListAsync<BankHolidayDTO>("[dbo].[sp_GetAllBankHolidaysById]", connection, parameters);
                    }
                    catch (Exception ex)
                    {
                        throw new DataAccessException("Error fetching bank holidays.", ex);
                    }
                    finally
                    {
                        _databaseWrapper.ReleaseConnection(connection);
                    }
                }
            }
            catch (MaximumConnectionsException ex)
            {
                throw new DataAccessException($"A maximum of '{_maxConnections}' are allowed.", ex);
            }
        }

        public async Task<bool> HasAny()
        {
            try
            {
                using (var connection = _databaseWrapper.GetConnection())
                {
                    using (var transaction = _databaseWrapper.BeginTransaction(connection))
                    {
                        try
                        {
                            var parameters = new DynamicParameters();
                            parameters.Add("HasAny", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                            await _databaseWrapper.ExecuteStoredProcedureAsync("[dbo].[sp_HasAnyHolidays]", parameters, connection, transaction);

                            return parameters.Get<bool>("HasAny");
                        }
                        catch(Exception ex)
                        {
                            throw new DataAccessException("Error processing holiday events", ex);
                        }
                        finally
                        {
                            _databaseWrapper.ReleaseConnection(connection);
                        }
                    }
                }
            }
            catch (MaximumConnectionsException ex)
            {
                throw new DataAccessException($"A maximum of '{_maxConnections}' are allowed.", ex);
            }
        }
    }
}
