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

public class Index_Auto_Militias_ById : Raven.Database.Linq.AbstractViewGenerator
{
	public Index_Auto_Militias_ById()
	{
		this.ViewText = @"from doc in docs.Militias
select new {
	Id = doc.Id
}";
		this.ForEntityNames.Add("Militias");
		this.AddMapDefinition(docs => 
			from doc in ((IEnumerable<dynamic>)docs)
			where string.Equals(doc["@metadata"]["Raven-Entity-Name"], "Militias", System.StringComparison.InvariantCultureIgnoreCase)
			select new {
				Id = doc.Id,
				__document_id = doc.__document_id
			});
		this.AddField("Id");
		this.AddField("__document_id");
		this.AddQueryParameterForMap("Id");
		this.AddQueryParameterForMap("__document_id");
		this.AddQueryParameterForReduce("Id");
		this.AddQueryParameterForReduce("__document_id");
	}
}
