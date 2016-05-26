<%
'=====================================================================================
'= CardinalCommerce (http://www.cardinalcommerce.com)
'= ccMenu.aspx
'= Version 6.0 03/01/2006
'=
'= Usage
'=		Handles link references to all API template pages
'=
'=====================================================================================
%>
<script runat="server">

Public Function randomOrderNumber()
	randomOrderNumber = New System.Random().Next(10000000, 99999999)
End Function

Public Function randomAmount()
	randomAmount = New System.Random().Next(2000, 99999)
End Function


</script>

<TABLE Width="750" cellpadding="2" cellspacing="2" border="0">
<TR bgcolor="#990000">
	<TD colspan="9"><font size="3" color="#ffffff"><b>Centinel - API Transactions</b></font></TD>
	<td align="center"><a href="demoStartPage.aspx"><font color="#ffffff">Demo</font></a></td>
</TR>
<TR bgcolor="#e0e0e0">
<TD align="center"><a href="ccLookup.aspx">Lookup/ Authenticate</a></TD><TD align="center" nowrap="true"><a href="ccAuthorize.aspx">Authorization</a><br/><a href="ccReAuthorize.aspx">Re-Authorization</a></TD><TD align="center"><a href="ccSale.aspx">Sale</a></TD><TD align="center"><a href="ccCapture.aspx">Capture</a></TD><TD align="center"><a href="ccVoid.aspx">Void</a></TD><TD align="center"><a href="ccRefund.aspx">Refund</a></TD><TD align="center"><a href="ccSearch.aspx">Search</a></TD><TD align="center"><a href="ccInitiatePayment.aspx">PreApproved Payments</a></TD><TD align="center"><a href="ccCancelAgreement.aspx">Cancel PreApproved Agreement</a></TD><TD align="center"><a href="ccAuthBridge.aspx">Auth-Bridge Lookup</a></TD>
</TR>
</TABLE>


