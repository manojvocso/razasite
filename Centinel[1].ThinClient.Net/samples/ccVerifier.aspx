<% @Page Language="VB" Explicit="True"%>
<%response.Expires=-1%>
<%response.Buffer=true%>
<%
'=====================================================================================
'= CardinalCommerce (http://www.cardinalcommerce.com)
'= ccVerifier.aspx
'= Version 6.0 03/21/2006
'= Purpose
'=		This page represents the TermUrl passed on the ccLaunch.aspx page. The
'=		external system will post the results of the transaction to this page.  
'=
'= Note
'=      This is ONLY A SAMPLE INTEGRATION. It has been tested under
'=		Internet Explorer(6.0), Mozilla (1.3, 1.7), Netscape (4.08,7.0), a production integration
'=		may require additional browser support and should be tested with respect to those guidelines.
'=====================================================================================
%>
<!-- #Include File="CentinelConfig.aspx"-->
<%@ import Namespace="CardinalCommerce" %>
<%

	Dim pares, merchant_data, redirectPage, strErrorNo, strErrorDesc
	Dim centinelRequest As New CentinelRequest()
	Dim centinelResponse As New CentinelResponse()

	'=====================================================================================
	' Retrieve the PaRes and MD values from the Card Issuer's Form POST to this Term URL page.
	' If you like, the MD data passed to the Card Issuer could contain the TransactionId
	' that would enable you to reestablish the transaction session. This would be the 
	' alternative to using the Client Session Cookies
	'=====================================================================================

	pares = request.Form("PaRes")
	merchant_data = request.Form("MD")
		
	'=====================================================================================
	' If the PaRes is Not Empty then process the cmpi_authenticate message
	'=====================================================================================

	 if (pares <> "") then

		centinelRequest.Add ("Version", Cstr(MessageVersion))
		centinelRequest.Add ("MsgType", "cmpi_authenticate")
		centinelRequest.Add ("TransactionId", Session("Centinel_TransactionId"))
		centinelRequest.Add ("TransactionType", Session("Centinel_TransactionType"))
		centinelRequest.Add ("ProcessorId", Cstr(ProcessorId))
		centinelRequest.Add ("MerchantId", Cstr(MerchantId))
		centinelRequest.Add ("TransactionPwd", Cstr(TransactionPwd))
		centinelRequest.Add ("PAResPayload", Cstr(pares))

		'=====================================================================================
		' Send the XML Msg to the MAPS Server
		' SendHTTP will send the cmpi_authenticate message to the MAPS Server (requires fully qualified Url)
		' The Response is the CentinelResponse Object
		'=====================================================================================

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
			

			'=====================================================================================
			' ************************************************************************************
			'			** Important Note **
			' ************************************************************************************
			'
			' Here you should persist the transaction results to your commerce system. 
			'
			' Be sure not to simply 'pass' the transaction results around from page to page on the
			' URL, since the values will pass thru the the consumers browser in the clear.
			' 
			'=====================================================================================

			' Using the centinelResponse object, we need to retrieve the results as follows

			Session("Centinel_PAResStatus") = centinelResponse.getValue("PAResStatus")
			Session("Centinel_SignatureVerification") = centinelResponse.getValue("SignatureVerification")
			Session("Centinel_ECI") = centinelResponse.getValue("EciFlag")
			Session("Centinel_XID") = centinelResponse.getValue("Xid")
			Session("Centinel_CAVV") = centinelResponse.getValue("Cavv")
			Session("Centinel_ErrorNo") = centinelResponse.getValue("ErrorNo")
			Session("Centinel_ErrorDesc") = centinelResponse.getValue("ErrorDesc")

			' Optional PayPal Response Values

			Session("Centinel_AddressOwnerName") = centinelResponse.getValue("AddressOwnerName")
			Session("Centinel_AddressStatus") = centinelResponse.getValue("AddressStatus")
			Session("Centinel_ConsumerStatus") = centinelResponse.getValue("ConsumerStatus")		
			Session("Centinel_ShippingAddress1") = centinelResponse.getValue("ShippingAddress1")
			Session("Centinel_ShippingAddress2") = centinelResponse.getValue("ShippingAddress2")
			Session("Centinel_ShippingCity") = centinelResponse.getValue("ShippingCity")
			Session("Centinel_ShippingState") = centinelResponse.getValue("ShippingState")
			Session("Centinel_ShippingPostalCode") = centinelResponse.getValue("ShippingPostalCode")
			Session("Centinel_ShippingCountryCode") = centinelResponse.getValue("ShippingCountryCode")

			' Optional SECURE-eBill Response Values

			Session("Centinel_AccountNumber") = centinelResponse.getValue("AccountNumber")
			Session("Centinel_ReferenceId") = centinelResponse.getValue("ReferenceId")

		End If

		centinelRequest = nothing
		centinelResponse = nothing
	else
		Session("Centinel_ErrorDesc") = "NO PARES RETURNED"
	end if

	'=====================================================================================
	' Initialize the redirect page to the Results Page. 
	' Production integrations will use the following handling checks to determine
	' where the consumer should be redirected.
	'=====================================================================================

	redirectPage = "ccResults.aspx"

	' Handle PayPal Payment Specific Logic

	If Session("Centinel_ErrorNo") = "0" AND Session("Centinel_SignatureVerification") = "N" Then

		Session("Message") = "Your transaction completed however is pending review. Your order will be shipped once payment is verified."
		redirectPage = "ccResults.aspx"

	ElseIf Session("Centinel_ErrorNo") = "0" AND Session("Centinel_SignatureVerification") = "Y" AND (Session("Centinel_PAResStatus") = "Y" OR Session("Centinel_PAResStatus") = "A") Then

		Session("Message") = "Your transaction completed successfully. Your order will be shipped immediately."
		redirectPage = "ccResults.aspx"

	ElseIf Session("Centinel_ErrorNo") = "0" AND Session("Centinel_SignatureVerification") = "Y" AND Session("Centinel_PAResStatus") = "P" Then

		Session("Message") = "Your transaction completed however payment is currently pending. Your order will be shipped once payment is verified."
		redirectPage = "ccResults.aspx"

	ElseIf Session("Centinel_ErrorNo") = "0" AND Session("Centinel_SignatureVerification") = "Y" AND Session("Centinel_PAResStatus") = "D" Then

		Session("Message") = "You cancelled your transaction for Data Reasons. Please update your information and select another form of payment."
		redirectPage = "ccResults.aspx"

	ElseIf Session("Centinel_ErrorNo") = "0" AND Session("Centinel_SignatureVerification") = "Y" AND Session("Centinel_PAResStatus") = "X" Then

		Session("Message") = "Your transaction was canceled prior to completion. Please provide another form of payment."
		redirectPage = "ccResults.aspx"

	Else
		' Unexpected State
		' Log Message Information
		' Verify payment was receive prior to shipping goods to consumer.

		Session("Message") = "Your completed however is pending review. Your order will be shipped once payment is verified."
		redirectPage = "ccResults.aspx"

	End If
	

%>
<HTML>
<HEAD>
<SCRIPT Language="Javascript">
<!--
	function onLoadHandler(){
		document.frmResultsPage.submit();
	}
//-->
</Script>
<Title>Processing Your Transaction</Title>
</HEAD>
<body onLoad="onLoadHandler();">

<FORM name="frmResultsPage" method="Post" action="<%=redirectPage%>" target="_parent">
<noscript>
	<br><br>
	<center>
	<font color="red">
	<h1>Processing Your Transaction</h1>
	<h2>JavaScript is currently disabled or is not supported by your browser.<br></h2>
	<h3>Please click Submit to continue the processing of your transaction.</h3>
	</font>
	<input type="submit" value="Submit">
	</center>
</noscript>
</FORM>
</BODY>
</HTML>
