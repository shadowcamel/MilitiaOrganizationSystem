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

public class Index_MilitiaIndex : Raven.Database.Linq.AbstractViewGenerator
{
	public Index_MilitiaIndex()
	{
		this.ViewText = @"from militia in docs.Militias
where militia.Group == ""2""
select new {
	Group = militia.Group,
	Id = militia.__document_id,
	InfoDic = militia.InfoDic
}";
		this.ForEntityNames.Add("Militias");
		this.AddMapDefinition(docs => 
			from militia in ((IEnumerable<dynamic>)docs)
			where string.Equals(militia["@metadata"]["Raven-Entity-Name"], "Militias", System.StringComparison.InvariantCultureIgnoreCase)
			where militia.Group == "2"
			select new {
				Group = militia.Group,
				Id = militia.__document_id,
				InfoDic = militia.InfoDic,
				__document_id = militia.__document_id
			});
		this.AddField("Group");
		this.AddField("Id");
		this.AddField("InfoDic");
		this.AddField("__document_id");
		this.AddQueryParameterForMap("Group");
		this.AddQueryParameterForMap("__document_id");
		this.AddQueryParameterForMap("InfoDic");
		this.AddQueryParameterForReduce("Group");
		this.AddQueryParameterForReduce("__document_id");
		this.AddQueryParameterForReduce("InfoDic");
	}
}
