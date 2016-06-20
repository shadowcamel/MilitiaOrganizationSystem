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

public class Index_Auto_Militias_ByGroupAndInfoDic_Name : Raven.Database.Linq.AbstractViewGenerator
{
	public Index_Auto_Militias_ByGroupAndInfoDic_Name()
	{
		this.ViewText = @"from doc in docs.Militias
select new {
	InfoDic_Name = doc.InfoDic.Name,
	Group = doc.Group
}";
		this.ForEntityNames.Add("Militias");
		this.AddMapDefinition(docs => 
			from doc in ((IEnumerable<dynamic>)docs)
			where string.Equals(doc["@metadata"]["Raven-Entity-Name"], "Militias", System.StringComparison.InvariantCultureIgnoreCase)
			select new {
				InfoDic_Name = doc.InfoDic.Name,
				Group = doc.Group,
				__document_id = doc.__document_id
			});
		this.AddField("InfoDic_Name");
		this.AddField("Group");
		this.AddField("__document_id");
		this.AddQueryParameterForMap("InfoDic.Name");
		this.AddQueryParameterForMap("Group");
		this.AddQueryParameterForMap("__document_id");
		this.AddQueryParameterForReduce("InfoDic.Name");
		this.AddQueryParameterForReduce("Group");
		this.AddQueryParameterForReduce("__document_id");
	}
}
