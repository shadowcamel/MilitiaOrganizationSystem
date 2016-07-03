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

public class Index_Militias_All : Raven.Database.Linq.AbstractViewGenerator
{
	public Index_Militias_All()
	{
		this.ViewText = @"from militia in docs.Militias
select new {
	Available = militia.Available,
	CadreAttendedCommittee = militia.CadreAttendedCommittee,
	CadreFullTime = militia.CadreFullTime,
	Education = militia.Education
}";
		this.ForEntityNames.Add("Militias");
		this.AddMapDefinition(docs => 
			from militia in ((IEnumerable<dynamic>)docs)
			where string.Equals(militia["@metadata"]["Raven-Entity-Name"], "Militias", System.StringComparison.InvariantCultureIgnoreCase)
			select new {
				Available = militia.Available,
				CadreAttendedCommittee = militia.CadreAttendedCommittee,
				CadreFullTime = militia.CadreFullTime,
				Education = militia.Education,
				__document_id = militia.__document_id
			});
		this.AddField("Available");
		this.AddField("CadreAttendedCommittee");
		this.AddField("CadreFullTime");
		this.AddField("Education");
		this.AddField("__document_id");
		this.AddQueryParameterForMap("Available");
		this.AddQueryParameterForMap("CadreAttendedCommittee");
		this.AddQueryParameterForMap("CadreFullTime");
		this.AddQueryParameterForMap("Education");
		this.AddQueryParameterForMap("__document_id");
		this.AddQueryParameterForReduce("Available");
		this.AddQueryParameterForReduce("CadreAttendedCommittee");
		this.AddQueryParameterForReduce("CadreFullTime");
		this.AddQueryParameterForReduce("Education");
		this.AddQueryParameterForReduce("__document_id");
	}
}
