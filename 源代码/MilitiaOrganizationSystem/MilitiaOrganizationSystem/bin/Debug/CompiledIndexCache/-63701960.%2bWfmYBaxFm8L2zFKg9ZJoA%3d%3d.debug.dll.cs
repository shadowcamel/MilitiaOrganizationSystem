using Raven.Abstractions;
using Raven.Database.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System;
using Raven.Database.Linq.PrivateExtensions;
using Lucene.Net.Documents;
using System.Globalization;
using System.Text.RegularExpressions;
using Raven.Database.Indexing;

public class Index_Auto_Militias_ByEquipmentInfo_MilitaryProfessionTypeNameAndGroupAndSex : Raven.Database.Linq.AbstractViewGenerator
{
	public Index_Auto_Militias_ByEquipmentInfo_MilitaryProfessionTypeNameAndGroupAndSex()
	{
		this.ViewText = @"from doc in docs.Militias
select new {
	EquipmentInfo_MilitaryProfessionTypeName = doc.EquipmentInfo_MilitaryProfessionTypeName,
	Group = doc.Group,
	Sex = doc.Sex
}";
		this.ForEntityNames.Add("Militias");
		this.AddMapDefinition(docs => 
			from doc in ((IEnumerable<dynamic>)docs)
			where string.Equals(doc["@metadata"]["Raven-Entity-Name"], "Militias", System.StringComparison.InvariantCultureIgnoreCase)
			select new {
				EquipmentInfo_MilitaryProfessionTypeName = doc.EquipmentInfo_MilitaryProfessionTypeName,
				Group = doc.Group,
				Sex = doc.Sex,
				__document_id = doc.__document_id
			});
		this.AddField("EquipmentInfo_MilitaryProfessionTypeName");
		this.AddField("Group");
		this.AddField("Sex");
		this.AddField("__document_id");
		this.AddQueryParameterForMap("EquipmentInfo_MilitaryProfessionTypeName");
		this.AddQueryParameterForMap("Group");
		this.AddQueryParameterForMap("Sex");
		this.AddQueryParameterForMap("__document_id");
		this.AddQueryParameterForReduce("EquipmentInfo_MilitaryProfessionTypeName");
		this.AddQueryParameterForReduce("Group");
		this.AddQueryParameterForReduce("Sex");
		this.AddQueryParameterForReduce("__document_id");
	}
}
