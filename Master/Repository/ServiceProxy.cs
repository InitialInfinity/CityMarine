using Master.API.Repository.Interface;
using Context;
using Master.Repository;
using Master.Repository.Interface;

namespace Master.API.Repository
{

    public static class ServiceProxy
    {
        private static DapperContext? _dapperContext;


        
        public static IParameterMasterRepository? parameterMasterRepository { get; internal set; }
        public static ICityMasterRepository? cityMasterRepository { get; internal set; }
        public static IStateMasterRepository? stateMasterRepository { get; internal set; }
        public static ICountryMasterRepository? countryMasterRepository { get; internal set; }
        public static IParameterValueMasterRepository? parameterValueMasterRepository { get; internal set; }

        public static ICurrencyMasterRepository? currencyMasterRepository { get; internal set; }

        public static ICurrencyRateMasterRepository? currencyRateMasterRepository { get; internal set; }
        public static ISubscriptionMasterRepository? subscriptionMasterRepository { get; internal set; }
        public static IServerAllocation? serverAllocationRepository { get; internal set; }
        public static ILoginDetailsRepository? loginDetailsRepository { get; internal set; }
        public static IRoleMasterRepository? roleMasterRepository { get; internal set; }
        public static IUnitConfigurationRepository? unitConfigurationRepository { get; internal set; }

        public static void Configure(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;

          
            parameterMasterRepository = new ParameterMasterRepository(_dapperContext);
            cityMasterRepository = new CityMasterRepository(_dapperContext);
            stateMasterRepository = new StateMasterRepository(_dapperContext);
            countryMasterRepository = new CountryMasterRepository(_dapperContext);
            parameterValueMasterRepository=new ParameterValueMasterRepository(_dapperContext);
            currencyMasterRepository=new CurrencyMasterRepository(_dapperContext);
            currencyRateMasterRepository=new CurrencyRateMasterRepository(_dapperContext);
            subscriptionMasterRepository = new SubscriptionMasterRepository(_dapperContext);
            serverAllocationRepository = new ServerAllocationRepository(_dapperContext);
            loginDetailsRepository = new LoginDetailsRepository(_dapperContext);
            roleMasterRepository = new RoleMasterRepository(_dapperContext);
            unitConfigurationRepository = new UnitConfigurationRepository(_dapperContext);
        }
    }
}
