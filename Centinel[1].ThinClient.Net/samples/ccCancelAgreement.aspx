<% @Page Language="VB" Explicit="True" %>
<%'======================================================================================
'=  CardinalCommerce (http://www.cardinalcommerce.com)
'=  ccCancelAgreement.aspx
'=  Version 6.0.0 03/21/2006
'=
'=  Usage
'=		Sample page to cancel a PayPal PreApproved Payment Agreement.
'======================================================================================%>
<!-- #Include File="CentinelConfig.aspx"-->
<%@ import Namespace="CardinalCommerce" %>
<%

Session("TID") = request("transaction_id")

%>
<HTML>
<Head>
<Title>Cancel PayPal Preapproved Agreement Page</Title>
</Head>
<body>
<!--#Include  File = "ccMenu.aspx"-->
<br/>
<b>Cancel PreApproved Payment Agreement Request Form</b>
<form name="frm" method="POST" action="ccCancelAgreement.aspx?execute=true">
<TABLE>
<TR bgcolor="ffff40">
	<TD>TransactionId </TD><TD><input type=text size="60" name="transaction_id" value="<%=Session("TID")%>"></TD>
</TR>
<TR bgcolor="ffff40">
	<TD>Description </TD><TD><input type=text size="60" name="description" value="Cancel Agreement Description"></TD>
</TR>
<TR>
  <TD></TD>
  <TD><input type=submit name="submit" value="Cancel Agreement"></TD>
</TR>
<TR>
  <TD colspan="2"><br><b><i>Required fields highlighted</i></b></TD>
</TR>
</table>
</form>
<br>
<hr>
<br><br>
<%

	Dim executeTransaction
	executeTransaction = request("execute")

	if (executeTransaction = "true") then

		Dim strErrorNo, strErrorDesc, strTransactionId, strStatusCode, strReasonCode
		Dim centinelRequest As New CentinelRequest()
		Dim centinelResponse As New CentinelResponse()

		'======================================================================================
		' Construct Message using Name / Value pairs
		'======================================================================================

		centinelRequest.Add ("Version", Cstr(MessageVersion))
		centinelRequest.Add ("MsgType", "cmpi_paypal_cancel_preapproved_agreement")
		centinelRequest.Add ("ProcessorId", Cstr(ProcessorId))
		centinelRequest.Add ("MerchantId", Cstr(MerchantId))
		centinelRequest.Add ("TransactionPwd", Cstr(TransactionPwd))
		centinelRequest.Add ("TransactionId", Cstr(request("transaction_id")))
		centinelRequest.Add ("Description", Cstr(request("description")))

		'=====================================================================================
		' Send the XML Msg to the MAPS Server
		' SendHTTP will send the cmpi_lookup message to the MAPS Server (requires fully qualified URL)
		' The Response is the CentinelResponse Object
		'=====================================================================================
		
		Try
			centinelResponse = centinelRequest.sendHTTP(Cstr(TransactionURL), Cstr(Timeout))
		
		Catch ex As Exception
           
			strErrorNo = "9040"
			strErrorDesc = "Communication Error"
		End Try

		If strErrorNo = "" Then
		
			strErrorNo = centinelResponse.getValue("ErrorNo")			 
			strErrorDesc = centinelResponse.getValue("ErrorDesc")
			strTransactionId = centinelResponse.getValue("TransactionId")
			strStatusCode = centinelResponse.getValue("StatusCode")
			strReasonCode = centinelResponse.getValue("ReasonCode")

		End If
		
		centinelResponse = nothing
		centinelRequest = nothing
%>


<b> Cancel PreApproved Payment Agreement Request Results </b>
<TABLE>
<TR>
	<TD>ProcessorId</TD>
	<TD><%=Cstr(ProcessorId)%></TD>
</TR>
<TR>
	<TD>MerchantId</TD>
	<TD><%=Cstr(MerchantId)%></TD>
</TR>
<TR>
	<TD>TransactionPwd</TD><TD><%=Cstr(TransactionPwd)%></TD>
</TR>
<TR>
	<TD>TransactionId</TD><TD><%=request("transaction_id")%></TD>
</TR>
<TR>
	<TD>Description</TD><TD><%=request("description")%></TD>
</TR>
</TABLE>
<br/><br/>
<b> Cancel PreApproved Payment Agreement Results </b>
<TABLE>
<TR>
	<TD>ErrorNo</TD><TD><%=strErrorNo%></TD>
</TR>
<TR>
	<TD>Description</TD><TD><%=strErrorDesc%></TD>
</TR>
<TR>
	<TD>Status Code</TD><TD><%=strStatusCode%></TD>
</TR>
<TR>
	<TD>Reason Code</TD><TD><%=strReasonCode%></TD>
</TR>
<TR>
	<TD>Transaction Id</TD><TD><%= strTransactionId %></TD>
</TR>
</TABLE>


<%

	End IF
%>


</body>
</html>

