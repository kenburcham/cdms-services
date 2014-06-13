using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NLog;
using services.Models;

/*
 * Manages Metadata for any type of metadata entity (Project, Dataset, etc.).  Usually called from 
 *  get, set in a model (See Project.Metadata.set/get)
 *  
 * Ken Burcham 4/9/14
 */ 

namespace services.Resources
{
    public class MetadataHelper
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        //if new metadata already exists but has a different value, update. if it doesn't exist, add it. (doesn't delete any existing).
        public static void saveMetadata(List<MetadataValue> currentMetadata, List<MetadataValue> newMetadata, int Id)
        {
            var db = ServicesContext.Current;
            //do nothing if our id isn't set
            if (Id == 0)
                throw new Exception("Metadata cannot be retrieved on an unsaved model (id is 0).");

            logger.Debug("Setting metadata for " + Id + ". total of : " + newMetadata.Count());

            foreach (var mv in newMetadata)
            {
                var matched = false;

                ICollection<MetadataValue> cmv_query = (from cmv in currentMetadata
                                                        where cmv.MetadataPropertyId == mv.MetadataPropertyId
                                                        select cmv).ToList();

                foreach (var cmv in cmv_query)
                {
                    //                        logger.Debug("->> looking at metadata: " + mv.MetadataPropertyId + "(" + mv.Id + ")");

                    if (cmv.MetadataPropertyId == mv.MetadataPropertyId)
                    {
                        matched = true;

                        //is it different than the existing value?  then save the new one
                        if (mv.Values != cmv.Values)
                        {
                            //logger.Debug("value changed! Saving updated effdt row.");

                            mv.EffDt = DateTime.Now;
                            mv.RelationId = Id;
                            db.MetadataValue.Add(mv);
                            db.SaveChanges();
                            break; //go to the next one
                        }
                        //                            else
                        //                                logger.Debug("metdata exists (match) but value was unchanged");

                    }
                }

                if (!matched)
                {
                    if (mv.Values != "")
                    {
                        //logger.Debug("wasn't matched so save it as a new one");
                        mv.EffDt = DateTime.Now;
                        mv.RelationId = Id;
                        db.MetadataValue.Add(mv);
                        db.SaveChanges();
                    }
                    //else
                    //    logger.Debug("value was empty -- don't save anything.");
                }

                //System.Diagnostics.Debug.Write("Ended with matched = "+matched);
            }
        }

        public static List<MetadataValue> getMetadata(int Id, int MetadataEntityTypeId)
        {

            var db = ServicesContext.Current;
            List<MetadataValue> results = new List<MetadataValue>();

            var q = from mv in db.MetadataValue
                    join mp in db.MetadataProperty on mv.MetadataPropertyId equals mp.Id
                    where mv.RelationId == Id
                    where mp.MetadataEntityId == MetadataEntityTypeId 
                    join mv2 in
                        (
                            from mi in db.MetadataValue
                            where mi.EffDt <= DateTime.Now
                            where mi.RelationId == Id
                            group mi by mi.MetadataPropertyId into cig
                            select new { MetadataPropertyId = cig.Key, EffDt = cig.Max(ed => ed.EffDt) }
                            ) on new { mv.MetadataPropertyId, mv.EffDt } equals new { mv2.MetadataPropertyId, mv2.EffDt }
                    select mv;

            foreach (var mv in q)
            {
                results.Add(mv);
            }

            return results;

        }

        //deletes metadata for an id of a certain type.
        public static void deleteMetadata(int Id, int MetadataEntityTypeId)
        {
            var db = ServicesContext.Current;
            if (Id == 0)
                return;

            var q = from mv in db.MetadataValue
                    join mp in db.MetadataProperty on mv.MetadataPropertyId equals mp.Id
                    where mv.RelationId == Id
                    where mp.MetadataEntityId == MetadataEntityTypeId 
                    select mv;

            foreach (var mv in q)
            {
                db.MetadataValue.Remove(mv);
            }

            db.SaveChanges();
        }

        //deletes ALL metadata regardless of type
        public static void deleteAllMetadata(int Id)
        {
            var db = ServicesContext.Current;
            if (Id == 0)
                return;

            var q = from mv in db.MetadataValue
                    join mp in db.MetadataProperty on mv.MetadataPropertyId equals mp.Id
                    where mv.RelationId == Id
                    select mv;

            foreach (var mv in q)
            {
                db.MetadataValue.Remove(mv);
            }

            db.SaveChanges();
        }


        /*public const string sql = @"select * from MetadataValues as mv
join MetadataProperties as mp on mp.Id = mv.MetadataPropertyId
where mv.RelationId = 1
and mp.MetadataEntityId = 1
and mv.EffDt = (
	select max(jmv.EffDt) from MetadataValues as jmv
	join MetadataProperties as jmp on jmp.Id = jmv.MetadataPropertyId
	where 
	
		jmv.RelationId = mv.RelationId
		AND jmp.MetadataEntityId = mp.MetadataEntityId
		AND jmv.MetadataPropertyId = mv.MetadataPropertyId
		AND jmv.EffDt <= GETDATE()
		)
		";
        */


    }
}