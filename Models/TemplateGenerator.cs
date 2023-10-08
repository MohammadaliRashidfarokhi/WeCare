namespace PCR.Models
{
    using System;
    using System.Text;

    /// <summary>
    /// Defines the <see cref="TemplateGenerator" />.
    /// </summary>
    public static class TemplateGenerator
    {
		/// <summary>
		/// The GetHTMLString.
		/// </summary>
		/// <param name="report">The report<see cref="Report"/>.</param>
		/// <returns>The <see cref="string"/>.</returns>

	
		public static string GetHTMLString(Report report )
        {
    

			var sb = new StringBuilder();
            sb.Append($@"
                        <html>
                            <head>

  
                                                                             </head>
                            <body>
                                <div class='header'><h1>{report.TestType} -Test</h1></div>
                             

"

      );


            sb.AppendFormat(@"    
		<div class=""stl_ stl_02"">
		<div class=""stl_view"">
			<div class=""stl_03 stl_04"">
				<div class=""stl_01"" style=""left:23.3233em;top:3.0631em;""><span class=""stl_05 stl_06 stl_07"">WeCare
						&nbsp;</span></div>
				<div class=""stl_01"" style=""left:3.0021em;top:5.5489em;""><span
						class=""stl_08 stl_06 stl_09""> <b> Name</b></span><span
						class=""stl_10 stl_06 stl_11"">_______________________ &nbsp;</span></div>
				<div class=""stl_01"" style=""left:3.0021em;top:6.7014em; ""><span class=""stl_08 stl_06 stl_12""
						style=""word-spacing:0.0213em;""> <b> Date of Lab </b> </span><span
						class=""stl_10 stl_06 stl_11"">________{8}__________ &nbsp;</span></div>
				<div class=""stl_01"" style=""left:3.0021em;top:7.8514em;""><span class=""stl_13 stl_06 stl_14""
						style=""word-spacing:-0.0058em;""> <b> Partner’s name: </b>  </span><span
						class=""stl_10 stl_06 stl_11"">_____{0}_______ &nbsp;</span></div>
				<div class=""stl_01"" style=""left:19.2192em;top:10.1531em;""><span class=""stl_08 stl_06 stl_15""
						style=""word-spacing:-0.0992em;"">  <b> Title: </b>  </span><span
						class=""stl_10 stl_06 stl_11"">________{7}-Test__________ &nbsp;</span></div>
				<div class=""stl_01"" style=""left:3.0021em;top:13.6047em;""><span class=""stl_08 stl_06 stl_16""
						style=""word-spacing:-0.0183em;"">To whom it may concern &nbsp;</span></div>
				<div class=""stl_01"" style=""left:3.0021em;top:15.9214em;""><span class=""stl_17 stl_18 stl_19""
						style=""word-spacing:-0.058em;"">This document is to certify that &nbsp;</span></div>
				<div class=""stl_01"" style=""left:3.0021em;top:18.2231em;""><span class=""stl_17 stl_18 stl_20""
						style=""word-spacing:-0.0428em;"">  <b> Patient Name:</b> {0} &nbsp;</span></div>
				<div class=""stl_01"" style=""left:3.0021em;top:20.5256em;""><span class=""stl_17 stl_18 stl_21""
						style=""word-spacing:-0.0431em;"">  <b> Passport number:</b> {1}&nbsp;</span></div>
				<div class=""stl_01"" style=""left:3.0021em;top:22.8272em;""><span class=""stl_17 stl_18 stl_22""
						style=""word-spacing:-0.0274em;"">  <b> Personal Number:</b> {2} &nbsp;</span></div>
				<div class=""stl_01"" style=""left:3.0021em;top:25.1297em;""><span class=""stl_17 stl_18 stl_23""
						style=""word-spacing:-0.0245em;"">  <b> Date of Birth:</b> {3} &nbsp;</span></div>
				<div class=""stl_01"" style=""left:3.0021em;top:27.4297em;""><span class=""stl_17 stl_18 stl_24"">   <b> Citizenship:</b> {4}
						&nbsp;</span></div>
				<div class=""stl_01"" style=""left:3.0021em;top:29.7322em;""><span class=""stl_17 stl_18 stl_25""
						style=""word-spacing:-0.0041em;"">  <b> Phone number:</b> {5} &nbsp;</span></div>
				<div class=""stl_01"" style=""left:3.0021em;top:32.0339em;""><span class=""stl_17 stl_18 stl_07""
						style=""word-spacing:-0.0466em;"">  <b> Email address: </b> {6} &nbsp;</span></div>
				<div class=""stl_01"" style=""left:3.0021em;top:34.3364em;""><span class=""stl_17 stl_18 stl_26""
						style=""word-spacing:-0.0356em;"">Has been tested </span><span class=""stl_27 stl_18 stl_28""
						style=""word-spacing:0.0194em;""> <strong>Result</strong> </span><span class=""stl_17 stl_18 stl_29""
						style=""word-spacing:-0.0292em;"">for <strong> {7} </strong>with a <strong>{7} </strong> test. &nbsp;</span></div>
			</div>
		</div>
	</div>

 <style>
        .stl_ sup {{
    vertical-align: baseline;
    position: relative;
    top: -0.4em;
}}

.stl_ sub {{
    vertical-align: baseline;
    position: relative;
    top: 0.4em;
}}

.stl_ a:link {{
    text-decoration: none;
}}

.stl_ a:visited {{
    text-decoration: none;
}}



.stl_ie {{
    font-size: 1pt;
}}

    .stl_ie body {{
        font-size: 12em;
    }}

@media print {{
    .stl_view {{
        font-size: 1em;
        transform: scale(1);
    }}
}}

.stl_grlink {{
    position: relative;
    width: 100%;
    height: 100%;
    z-index: 1000000;
}}

.stl_01 {{
    position: absolute;
    white-space: nowrap;
}}

.stl_02 {{
    font-size: 1em;
    line-height: 0.0em;
    width: 51em;
    height: 37em;
    border-style: none;
    display: block;
    margin: 0em;
}}

@supports(-ms-ime-align:auto) {{
    .stl_02 {{
        overflow: hidden;
    }}
}}

.stl_03 {{
    position: relative;
    width: 51em;
}}

.stl_04 {{
    height: 6.6em;
}}

.stl_ie .stl_04 {{
    height: 66em;
}}

.stl_05 {{
    font-size: 1.15em;
    font-family: """"HDBJEE+Arial Bold"""";
    color: #000000;
}}

.stl_06 {{
    line-height: 1.117188em;
}}

.stl_07 {{
    letter-spacing: 0.0089em;
}}

.stl_ie .stl_07 {{
    letter-spacing: 0.1638px;
}}

.stl_08 {{
    font-size: 1em;
    font-family: """"HDBJEE+Arial Bold"""";
    color: #000000;
}}

.stl_09 {{
    letter-spacing: -0.0049em;
}}

.stl_ie .stl_09 {{
    letter-spacing: -0.0779px;
}}

.stl_10 {{
    font-size: 1em;
    font-family: """"NTJTKW+Arial"""";
    color: #000000;
}}

.stl_11 {{
    letter-spacing: -0.0062em;
}}

.stl_ie .stl_11 {{
    letter-spacing: -0.0984px;
}}

.stl_12 {{
    letter-spacing: -0.0067em;
}}

.stl_ie .stl_12 {{
    letter-spacing: -0.1071px;
}}

.stl_13 {{
    font-size: 1em;
    font-family: """"QVQVKC+Arial Bold"""";
    color: #000000;
}}

.stl_14 {{
    letter-spacing: 0.0008em;
}}

.stl_ie .stl_14 {{
    letter-spacing: 0.0131px;
}}

.stl_15 {{
    letter-spacing: 0.0192em;
}}

.stl_ie .stl_15 {{
    letter-spacing: 0.3072px;
}}

.stl_16 {{
    letter-spacing: 0.0025em;
}}

.stl_ie .stl_16 {{
    letter-spacing: 0.0396px;
}}

.stl_17 {{
    font-size: 1em;
    font-family: """"QMTPCJ+Times New Roman"""";
    color: #000000;
}}

.stl_18 {{
    line-height: 1.107422em;
}}

.stl_19 {{
    letter-spacing: 0.0093em;
}}

.stl_ie .stl_19 {{
    letter-spacing: 0.1486px;
}}

.stl_20 {{
    letter-spacing: 0.007em;
}}

.stl_ie .stl_20 {{
    letter-spacing: 0.1121px;
}}

.stl_21 {{
    letter-spacing: 0.0071em;
}}

.stl_ie .stl_21 {{
    letter-spacing: 0.1143px;
}}

.stl_22 {{
    letter-spacing: 0.0039em;
}}

.stl_ie .stl_22 {{
    letter-spacing: 0.0617px;
}}

.stl_23 {{
    letter-spacing: 0.0051em;
}}

.stl_ie .stl_23 {{
    letter-spacing: 0.0809px;
}}

.stl_24 {{
    letter-spacing: 0.0092em;
}}

.stl_ie .stl_24 {{
    letter-spacing: 0.147px;
}}

.stl_25 {{
    letter-spacing: 0.0046em;
}}

.stl_ie .stl_25 {{
    letter-spacing: 0.0743px;
}}

.stl_26 {{
    letter-spacing: 0.0056em;
}}

.stl_ie .stl_26 {{
    letter-spacing: 0.0892px;
}}

.stl_27 {{
    font-size: 1em;
    font-family: """"IWIEBD+Times New Roman Bold"""", """"Times New Roman"""";
    color: #000000;
}}

.stl_28 {{
    letter-spacing: 0.0016em;
}}

.stl_ie .stl_28 {{
    letter-spacing: 0.0251px;
}}

.stl_29 {{
    letter-spacing: 0.0111em;
}}

.stl_ie .stl_29 {{
    letter-spacing: 0.1781px;
}}

.stl_01 {{
    padding-bottom: 10px
}}
    </style>
                      "


              , report.Patient, report.PassportNumber, report.PersonalIdentityNumber, report.BirthDate.ToString("d"), report.Citizenship, report.Phonenumber, report.Email, report.TestType, DateTime.Now);

            sb.Append(@"
                           
                            </body>
                        </html>");
            return sb.ToString();
        }
    }
}
