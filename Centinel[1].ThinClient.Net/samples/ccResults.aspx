<% @Page Language="VB" Explicit="True" %>
<%response.Expires=-1%>
<%response.Buffer=true%>
<%
'=====================================================================================
'= CardinalCommerce (http://www.cardinalcommerce.com)
'= ccResults.aspx
'= Version 6.0 03/21/2006
'= Usage
'=		This page is used to simply display the results of the transaction processing. 
'=		This page simulates a order confirmation page.
'=
'=====================================================================================
%>
<!--#Include  File = "ccMenu.aspx"-->

<html>

<Head>
<Title>Transaction Results Page</Title>
</Head>

<body>

<br/>

<b>Transaction Results Page</b>
<br>
Intended to simulate an order confirmation page.

<% If Len(Session("Message")) > 0 then %>
<br/><br/>
	<font color="red"><b>Sample Message : <%=Session("Message")%></b></font>
<br/><br/>
<% End If %>

<TABLE id="results">
<TR>
	<TD>Enrolled : </TD>
	<TD><%=Session("Centinel_Enrolled")%></TD>
</TR>
<TR>
	<TD>Transaction Status :</TD>
	<TD><%=Session("Centinel_PAResStatus")%></TD>
</TR>
<TR>
	<TD>Signature Verification :</TD>
	<TD><%=Session("Centinel_SignatureVerification")%></TD>
</TR>
<TR>
	<TD>ECI :</TD>
	<TD><%=Session("Centinel_ECI")%></TD>
</TR>
<TR>
	<TD>Error No :</TD>
	<TD><%=Session("Centinel_ErrorNo")%></TD>
</TR>
<TR>
	<TD>Error Desc : </TD>
	<TD><%=Session("Centinel_ErrorDesc")%></TD>
</TR>
<TR>
	<TD>Transaction Id :</TD>
	<TD><%=Session("Centinel_TransactionId")%></TD>
</TR>
<TR>
	<TD><b>Optional PayPal Return Values</b></TD>
	<TD></TD>
</TR>
<TR>
	<TD>Address Owner Name :</TD>
	<TD><%=Session("Centinel_AddressOwnerName")%></TD>
</TR>
<TR>
	<TD>Address Status :</TD>
	<TD><%=Session("Centinel_AddressStatus")%></TD>
</TR>
<TR>
	<TD>Consumer Status :</TD>
	<TD><%=Session("Centinel_ConsumerStatus")%></TD>
</TR>
<TR>
	<TD>Shipping Address1 :</TD>
	<TD><%=Session("Centinel_ShippingAddress1")%></TD>
</TR>
<TR>
	<TD>Shipping Address2 :</TD>
	<TD><%=Session("Centinel_ShippingAddress2")%></TD>
</TR>
<TR>
	<TD>Shipping City :</TD>
	<TD><%=Session("Centinel_ShippingCity")%></TD>
</TR>
<TR>
	<TD>Shipping State :</TD>
	<TD><%=Session("Centinel_ShippingState")%></TD>
</TR>
<TR>
	<TD>Shipping Postal Code :</TD>
	<TD><%=Session("Centinel_ShippingPostalCode")%></TD>
</TR>
<TR>
	<TD>Shipping Country Code :</TD>
	<TD><%=Session("Centinel_ShippingCountryCode")%></TD>
</TR>
<TR>
	<TD><b>Optional SECURE-eBill Return Values</b></TD>
	<TD></TD>
</TR>
<TR>
	<TD>Account Number :</TD>
	<TD><%=Session("Centinel_AccountNumber")%></TD>
</TR>
<TR>
	<TD>Reference Id :</TD>
	<TD><%=Session("Centinel_ReferenceId")%></TD>
</TR>
</TABLE>

<%
'======================================================================================
' Remove All the Session Contents Once the Transaction Is Complete
'======================================================================================

Session.Contents.RemoveAll()

%>
</body>
</HTML>








