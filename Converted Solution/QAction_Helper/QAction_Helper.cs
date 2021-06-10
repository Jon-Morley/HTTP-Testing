// --- auto-generated code --- do not modify ---
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Skyline.DataMiner.Scripting
{
public static class Parameter
{
	/// <summary>PID: 1081 | Type: read</summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	public const int http_debug_1081 = 1081;
	/// <summary>PID: 1081 | Type: read</summary>
	public const int http_debug = 1081;
	public class Write
	{
		/// <summary>PID: 1080 | Type: write</summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public const int button_poll_endpoints_1080 = 1080;
		/// <summary>PID: 1080 | Type: write</summary>
		public const int button_poll_endpoints = 1080;
	}
	public class Endpoints
	{
		/// <summary>PID: 1000</summary>
		public const int tablePid = 1000;
		/// <summary>IDX: 0</summary>
		public const int indexColumn = 0;
		/// <summary>PID: 1001</summary>
		public const int indexColumnPid = 1001;
		public class Pid
		{
			/// <summary>PID: 1001 | Type: read</summary>
			[EditorBrowsable(EditorBrowsableState.Never)]
			public const int endpointsinstance_1001 = 1001;
			/// <summary>PID: 1001 | Type: read</summary>
			public const int endpointsinstance = 1001;
			/// <summary>PID: 1002 | Type: read</summary>
			[EditorBrowsable(EditorBrowsableState.Never)]
			public const int endpoints_name_1002 = 1002;
			/// <summary>PID: 1002 | Type: read</summary>
			public const int endpoints_name = 1002;
			/// <summary>PID: 1003 | Type: read</summary>
			[EditorBrowsable(EditorBrowsableState.Never)]
			public const int endpoints_ipv4_1003 = 1003;
			/// <summary>PID: 1003 | Type: read</summary>
			public const int endpoints_ipv4 = 1003;
			/// <summary>PID: 1004 | Type: read</summary>
			[EditorBrowsable(EditorBrowsableState.Never)]
			public const int endpoints_nginx_api_1004 = 1004;
			/// <summary>PID: 1004 | Type: read</summary>
			public const int endpoints_nginx_api = 1004;
			/// <summary>PID: 1005 | Type: read</summary>
			[EditorBrowsable(EditorBrowsableState.Never)]
			public const int endpoints_http_response_1005 = 1005;
			/// <summary>PID: 1005 | Type: read</summary>
			public const int endpoints_http_response = 1005;
			public class Write
			{
			}
		}
		public class Idx
		{
			/// <summary>IDX: 0 | Type: read</summary>
			[EditorBrowsable(EditorBrowsableState.Never)]
			public const int endpointsinstance_1001 = 0;
			/// <summary>IDX: 0 | Type: read</summary>
			public const int endpointsinstance = 0;
			/// <summary>IDX: 1 | Type: read</summary>
			[EditorBrowsable(EditorBrowsableState.Never)]
			public const int endpoints_name_1002 = 1;
			/// <summary>IDX: 1 | Type: read</summary>
			public const int endpoints_name = 1;
			/// <summary>IDX: 2 | Type: read</summary>
			[EditorBrowsable(EditorBrowsableState.Never)]
			public const int endpoints_ipv4_1003 = 2;
			/// <summary>IDX: 2 | Type: read</summary>
			public const int endpoints_ipv4 = 2;
			/// <summary>IDX: 3 | Type: read</summary>
			[EditorBrowsable(EditorBrowsableState.Never)]
			public const int endpoints_nginx_api_1004 = 3;
			/// <summary>IDX: 3 | Type: read</summary>
			public const int endpoints_nginx_api = 3;
			/// <summary>IDX: 4 | Type: read</summary>
			[EditorBrowsable(EditorBrowsableState.Never)]
			public const int endpoints_http_response_1005 = 4;
			/// <summary>IDX: 4 | Type: read</summary>
			public const int endpoints_http_response = 4;
		}
	}
}
public class WriteParameters
{
	/// <summary>PID: 1080  | Type: write | DISCREETS: Poll Endpoints = 1</summary>
	public System.Object Button_poll_endpoints {get { return Protocol.GetParameter(1080); }set { Protocol.SetParameter(1080, value); }}
	public SLProtocolExt Protocol;
	public WriteParameters(SLProtocolExt protocol)
	{
		Protocol = protocol;
	}
}
public interface SLProtocolExt : SLProtocol
{
	/// <summary>PID: 1000</summary>
	EndpointsQActionTable endpoints { get; set; }
	object Afterstartup_dummy { get; set; }
	object Endpointsinstance_1001 { get; set; }
	object Endpointsinstance { get; set; }
	object Endpoints_name_1002 { get; set; }
	object Endpoints_name { get; set; }
	object Endpoints_ipv4_1003 { get; set; }
	object Endpoints_ipv4 { get; set; }
	object Endpoints_nginx_api_1004 { get; set; }
	object Endpoints_nginx_api { get; set; }
	object Endpoints_http_response_1005 { get; set; }
	object Endpoints_http_response { get; set; }
	object Button_poll_endpoints_1080 { get; set; }
	object Button_poll_endpoints { get; set; }
	object Http_debug_1081 { get; set; }
	object Http_debug { get; set; }
	WriteParameters Write { get; set; }
}
public class ConcreteSLProtocolExt : ConcreteSLProtocol, SLProtocolExt
{
	/// <summary>PID: 1000</summary>
	public EndpointsQActionTable endpoints { get; set; }
	/// <summary>PID: 2  | Type: dummy</summary>
	public System.Object Afterstartup_dummy {get { return GetParameter(2); }set { SetParameter(2, value); }}
	/// <summary>PID: 1001  | Type: read</summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	public System.Object Endpointsinstance_1001 {get { return GetParameter(1001); }set { SetParameter(1001, value); }}
	/// <summary>PID: 1001  | Type: read</summary>
	public System.Object Endpointsinstance {get { return GetParameter(1001); }set { SetParameter(1001, value); }}
	/// <summary>PID: 1002  | Type: read</summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	public System.Object Endpoints_name_1002 {get { return GetParameter(1002); }set { SetParameter(1002, value); }}
	/// <summary>PID: 1002  | Type: read</summary>
	public System.Object Endpoints_name {get { return GetParameter(1002); }set { SetParameter(1002, value); }}
	/// <summary>PID: 1003  | Type: read</summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	public System.Object Endpoints_ipv4_1003 {get { return GetParameter(1003); }set { SetParameter(1003, value); }}
	/// <summary>PID: 1003  | Type: read</summary>
	public System.Object Endpoints_ipv4 {get { return GetParameter(1003); }set { SetParameter(1003, value); }}
	/// <summary>PID: 1004  | Type: read</summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	public System.Object Endpoints_nginx_api_1004 {get { return GetParameter(1004); }set { SetParameter(1004, value); }}
	/// <summary>PID: 1004  | Type: read</summary>
	public System.Object Endpoints_nginx_api {get { return GetParameter(1004); }set { SetParameter(1004, value); }}
	/// <summary>PID: 1005  | Type: read</summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	public System.Object Endpoints_http_response_1005 {get { return GetParameter(1005); }set { SetParameter(1005, value); }}
	/// <summary>PID: 1005  | Type: read</summary>
	public System.Object Endpoints_http_response {get { return GetParameter(1005); }set { SetParameter(1005, value); }}
	/// <summary>PID: 1080  | Type: write | DISCREETS: Poll Endpoints = 1</summary>
	public System.Object Button_poll_endpoints_1080 {get { return GetParameter(1080); }set { SetParameter(1080, value); }}
	/// <summary>PID: 1080  | Type: write | DISCREETS: Poll Endpoints = 1</summary>
	public System.Object Button_poll_endpoints {get { return Write.Button_poll_endpoints; }set { Write.Button_poll_endpoints = value; }}
	/// <summary>PID: 1081  | Type: read</summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	public System.Object Http_debug_1081 {get { return GetParameter(1081); }set { SetParameter(1081, value); }}
	/// <summary>PID: 1081  | Type: read</summary>
	public System.Object Http_debug {get { return GetParameter(1081); }set { SetParameter(1081, value); }}
	public WriteParameters Write { get; set; }
	public ConcreteSLProtocolExt()
	{
		endpoints = new EndpointsQActionTable(this, 1000, "endpoints");
		Write = new WriteParameters(this);
	}
}
/// <summary>IDX: 0</summary>
public class EndpointsQActionTable : QActionTable, IEnumerable<EndpointsQActionRow>
{
	public EndpointsQActionTable(SLProtocol protocol, int tableId, string tableName) : base(protocol, tableId, tableName) { }
	IEnumerator IEnumerable.GetEnumerator() { return (IEnumerator) GetEnumerator(); }
	public IEnumerator<EndpointsQActionRow> GetEnumerator() { return new QActionTableEnumerator<EndpointsQActionRow>(this); }
}
/// <summary>IDX: 0</summary>
public class EndpointsQActionRow : QActionTableRow
{
	/// <summary>PID: 1001 | Type: read</summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	public System.Object Endpointsinstance_1001 { get { if (base.Columns.ContainsKey(0)) { return base.Columns[0]; } else { return null; } } set { if (base.Columns.ContainsKey(0)) { base.Columns[0] = value; } else { base.Columns.Add(0, value); } } }
	/// <summary>PID: 1001 | Type: read</summary>
	public System.Object Endpointsinstance { get { if (base.Columns.ContainsKey(0)) { return base.Columns[0]; } else { return null; } } set { if (base.Columns.ContainsKey(0)) { base.Columns[0] = value; } else { base.Columns.Add(0, value); } } }
	/// <summary>PID: 1002 | Type: read</summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	public System.Object Endpoints_name_1002 { get { if (base.Columns.ContainsKey(1)) { return base.Columns[1]; } else { return null; } } set { if (base.Columns.ContainsKey(1)) { base.Columns[1] = value; } else { base.Columns.Add(1, value); } } }
	/// <summary>PID: 1002 | Type: read</summary>
	public System.Object Endpoints_name { get { if (base.Columns.ContainsKey(1)) { return base.Columns[1]; } else { return null; } } set { if (base.Columns.ContainsKey(1)) { base.Columns[1] = value; } else { base.Columns.Add(1, value); } } }
	/// <summary>PID: 1003 | Type: read</summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	public System.Object Endpoints_ipv4_1003 { get { if (base.Columns.ContainsKey(2)) { return base.Columns[2]; } else { return null; } } set { if (base.Columns.ContainsKey(2)) { base.Columns[2] = value; } else { base.Columns.Add(2, value); } } }
	/// <summary>PID: 1003 | Type: read</summary>
	public System.Object Endpoints_ipv4 { get { if (base.Columns.ContainsKey(2)) { return base.Columns[2]; } else { return null; } } set { if (base.Columns.ContainsKey(2)) { base.Columns[2] = value; } else { base.Columns.Add(2, value); } } }
	/// <summary>PID: 1004 | Type: read</summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	public System.Object Endpoints_nginx_api_1004 { get { if (base.Columns.ContainsKey(3)) { return base.Columns[3]; } else { return null; } } set { if (base.Columns.ContainsKey(3)) { base.Columns[3] = value; } else { base.Columns.Add(3, value); } } }
	/// <summary>PID: 1004 | Type: read</summary>
	public System.Object Endpoints_nginx_api { get { if (base.Columns.ContainsKey(3)) { return base.Columns[3]; } else { return null; } } set { if (base.Columns.ContainsKey(3)) { base.Columns[3] = value; } else { base.Columns.Add(3, value); } } }
	/// <summary>PID: 1005 | Type: read</summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	public System.Object Endpoints_http_response_1005 { get { if (base.Columns.ContainsKey(4)) { return base.Columns[4]; } else { return null; } } set { if (base.Columns.ContainsKey(4)) { base.Columns[4] = value; } else { base.Columns.Add(4, value); } } }
	/// <summary>PID: 1005 | Type: read</summary>
	public System.Object Endpoints_http_response { get { if (base.Columns.ContainsKey(4)) { return base.Columns[4]; } else { return null; } } set { if (base.Columns.ContainsKey(4)) { base.Columns[4] = value; } else { base.Columns.Add(4, value); } } }
	public EndpointsQActionRow() : base(0, 5) { }
	public EndpointsQActionRow(System.Object[] oRow) : base(0, 5, oRow) { }
	public static implicit operator EndpointsQActionRow(System.Object[] source) { return new EndpointsQActionRow(source); }
	public static implicit operator System.Object[](EndpointsQActionRow source) { return source.ToObjectArray(); }
}
}
