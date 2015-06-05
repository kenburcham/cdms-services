using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace services.Resources
{
    public class Metadata
    {

        public const string sql = @"select * from MetadataValues as mv
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


    }
}