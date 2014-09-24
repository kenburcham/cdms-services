using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using services.Models.Data;

namespace services.Models
{
    public class ServicesContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, add the following
        // code to the Application_Start method in your Global.asax file.
        // Note: this will destroy and re-create your database with every model change.
        // 
        // System.Data.Entity.Database.SetInitializer(new System.Data.Entity.DropCreateDatabaseIfModelChanges<services.Models.ServicesContext>());

        public ServicesContext() : base("name=ServicesContext")
        {
            this.Configuration.LazyLoadingEnabled = true;
            this.Configuration.ProxyCreationEnabled = true;  

        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<MetadataValue> MetadataValue { get; set; }
        public DbSet<AuditJournal> AuditJournal { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Location> Location { get; set; }
        public DbSet<LocationType> LocationType { get; set; }
        public DbSet<MetadataEntity> MetadataEntity { get; set; }
        public DbSet<MetadataProperty> MetadataProperty { get; set; }
        public DbSet<Organization> Organization { get; set; }
        public DbSet<ProjectType> ProjectType { get; set; }
        public DbSet<SdeFeatureClass> SdeFeatureClass { get; set; }
        public DbSet<UserPreference> UserPreference { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<FileType> FileTypes { get; set; }
        public DbSet<Department> Departments { get; set; }

        public DbSet<Dataset> Datasets { get; set; }
        public DbSet<Datastore> Datastores { get; set; }
        public DbSet<DatasetField> DatasetFields { get; set; }
        public DbSet<Field> Fields { get; set; }
        public DbSet<FieldCategory> FieldCategories { get; set; }
        public DbSet<FieldRole> FieldRoles { get; set; }
        public DbSet<Instrument> Instruments { get; set; }
        public DbSet<InstrumentType> InstrumentType { get; set; }
        public DbSet<InstrumentAccuracyCheck> AccuracyChecks { get; set; }
        public DbSet<Source> Sources { get; set; }
        public DbSet<QAStatus> QAStatuses { get; set; }
        public DbSet<WaterBody> WaterBodies { get; set; }

        public DbSet<Activity> Activities { get; set; }
        public DbSet<ActivityQA> ActivityQAs { get; set; }
        public DbSet<ActivityType> ActivityTypes { get; set; }

        public DbSet<AdultWeir_Detail> AdultWeir_Detail { get; set; }
        public DbSet<AdultWeir_Header> AdultWeir_Header { get; set; }

        public DbSet<FishTransport_Detail> FishTransport_Detail { get; set; }
        public DbSet<FishTransport_Header> FishTransport_Header { get; set; }

        public DbSet<WaterTemp_Detail> WaterTemp_Detail { get; set; }
        public DbSet<WaterTemp_Header> WaterTemp_Header { get; set; }

        public DbSet<Appraisal_Detail> Appraisal_Detail { get; set; }
        public DbSet<Appraisal_Header> Appraisal_Header { get; set; }

        public DbSet<ArtificialProduction_Header> ArtificialProduction_Header { get; set; }
        public DbSet<ArtificialProduction_Detail> ArtificialProduction_Detail { get; set; }

        public DbSet<CreelSurvey_Header> CreelSurvey_Header { get; set; }
        public DbSet<CreelSurvey_Detail> CreelSurvey_Detail { get; set; }
        public DbSet<CreelSurvey_Carcass> CreelSurvey_Carcass { get; set; }

        //get the dbset by name rather than by type
        public DbSet GetDbSet(string entityName)
        {
            return this.Set(GetTypeFor(entityName));
        }

        public System.Type GetTypeFor(string entityName)
        {
            var datasource = "services.Models.Data." + entityName;
            var obj = System.Activator.CreateInstance("services", datasource).Unwrap();
            return obj.GetType();
        }

        public dynamic GetObjectFor(string entityName)
        {
            var datasource = "services.Models.Data." + entityName;
            var obj = System.Activator.CreateInstance("services", datasource).Unwrap();
            return obj;
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            //in EFF 5 this is the only way to specify decimal precision
            modelBuilder.Entity<Location>().Property(p => p.GPSEasting).HasPrecision(18, 8);
            modelBuilder.Entity<Location>().Property(p => p.GPSNorthing).HasPrecision(18, 8);
            modelBuilder.Entity<Location>().Property(p => p.Latitude).HasPrecision(18, 13);
            modelBuilder.Entity<Location>().Property(p => p.Longitude).HasPrecision(18, 13);
            modelBuilder.Entity<Location>().Property(p => p.RiverMile).HasPrecision(5, 2);

        }


        public static ServicesContext Current
        {
            get {
                if (System.Web.HttpContext.Current != null) //hey because sometimes it is! TODO
                    return System.Web.HttpContext.Current.Items["_EntityContext"] as ServicesContext;
                else
                    return new ServicesContext();
                }
                
        }

        public static ServicesContext RestartCurrent
        {
            get
            {
                //dispose of the existing one if it exists.
                var entityContext = System.Web.HttpContext.Current.Items["_EntityContext"] as ServicesContext;
                if (entityContext != null)
                    entityContext.Dispose();

                //start a new one.
                System.Web.HttpContext.Current.Items["_EntityContext"] = new ServicesContext(); //create a whole new one...
                return System.Web.HttpContext.Current.Items["_EntityContext"] as ServicesContext; 
            }

        }

    }
}
