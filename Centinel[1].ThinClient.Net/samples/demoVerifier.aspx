<% @Page Language="VB" Explicit="True"%>
<%response.Expires=-1%>
<%response.Buffer=true%>
<%@ import Namespace="CardinalCommerce" %>
<!-- #Include File="CentinelConfig.aspx"-->
<%
'========================================================================================
'=  Cardinal Commerce (http://www.cardinalcommerce.com)
'=	demoVerifier.aspx
'=  Version 6.0
'=	03/01/2006
'=
'=	The Card Issuer will post the results of the authentication to this page. This page 
'=  will Pass the PARes to the MAPS for validation of the PARes and will return the XID, CAVV, ECI, 
'=	Authentication Status and Signature values. 
'=
'=	Checking these values will determine what the next step in the flow should be. If the 
'=	authentication is successful then the CAVV, ECI, and XID values should be passed
'=	on the authorization message. If the authentication was unsuccessful, or resulted
'=	in an error the consumer should be prompted for another form of payment.
'=
'========================================================================================
%>

<%

	Dim pares, merchant_data, redirectPage
	Dim centinelRequest As New CentinelRequest()
	Dim centinelResponse As New CentinelResponse()
	
	redirectPage = "demoConfirmation.aspx"

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

		Try
		
			centinelResponse = centinelRequest.sendHTTP(Cstr(TransactionURL), Cstr(Timeout))

		Catch ex As Exception

			Session("Message") = "Your transaction was not able to complete, please choose another form of payment."
			redirectPage = "demoCheckout.aspx"

		End Try
		
		'=====================================================================================
		' ************************************************************************************
		'								** Important Note **
		' ************************************************************************************
		'
		' Here you should persist the authentication results to your commerce system. 
		'
		' Be sure not to simply 'pass' the authentication results around from page to page on the
		' URL , since the values could be easily spoofed if that technique is used.
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
	
		' Store optional PayPal response values if the transaction is a PayPal transaction

		If Session("Centinel_TransactionType") = "P" OR Session("Centinel_TransactionType") = "X" Then
			Session("Centinel_ShippingName") = centinelResponse.getValue("ConsumerName")
			Session("Centinel_Email") = centinelResponse.getValue("EMail")
			Session("Centinel_PayPalAddressStatus") = centinelResponse.getValue("AddressStatus")
			Session("Centinel_PayPalConsumerStatus") = centinelResponse.getValue("ConsumerStatus")
			Session("Centinel_ShippingAddress") = centinelResponse.getValue("ShippingAddress1")
			Session("Centinel_ShippingAddress2") = centinelResponse.getValue("ShippingAddress2")
			Session("Centinel_ShippingCity") = centinelResponse.getValue("ShippingCity")
			Session("Centinel_ShippingState") = centinelResponse.getValue("ShippingState")
			Session("Centinel_ShippingPostalCode") = centinelResponse.getValue("ShippingPostalCode")
			Session("Centinel_ShippingCountryCode") = centinelResponse.getValue("ShippingCountryCode")
		End If

		' Optional Secure-eBill Response Values

		Session("Centinel_AccountNumber") = centinelResponse.getValue("AccountNumber")
		Session("Centinel_ReferenceId") = centinelResponse.getValue("ReferenceId")
		
		
		centinelRequest = nothing
		centinelResponse = nothing
	else
		Session("Centinel_ErrorDesc") = "NO PARES RETURNED"
	end if

	'=====================================================================================
	' Determine if the result was Successful or Error
	'
	' If the Authentication results (PAResStatus) is a Y or A, and the SignatureVerification is Y, then 
	' the Payer Authentication was successful. The Authorization Message should be processed, 
	' and the User taken to a Order Confirmation location.
	'
	' If the Authentication results were not successful, the Authentication results were N, U or
	' the ErrorNo was NOT '0' then the Consumer should be redirected, and prompted for another 
	' form of payment.
	'
	' Note that it is also important that you account for cases when your flow logic can account 
	' for error cases, and the flow can be broken after 'N' number of attempts
	'
	'=====================================================================================


	'=====================================================================================
	' Determine if the PayPal result was Successful or Error
	'
	' If the Authentication result (PAResStatus) is Y then 
	' the Payer Authentication was successful. The Authorization Message should be processed, 
	' and the User taken to an Order Completion location.
	'
	' If the Authentication results were not successful, the Authentication results were P, X or
	' the ErrorNo was NOT '0' then the Consumer should be redirected, and prompted for another 
	' form of payment.
	'
	' Note that it is also important that you account for cases when your flow logic can account 
	' for error cases, and the flow can be broken after 'N' number of attempts
	'
	'=====================================================================================
	
	If Session("Centinel_TransactionType") = "P" Then
	
		If Session("Centinel_PAResStatus") = "Y" Then
		
			Session("Message") = "Your transaction completed successfully. Your order will be shipped immediately."
			redirectPage = "demoOrderComplete.aspx"
			
		ElseIf Session("Centinel_PAResStatus") = "P" Then
		
			Session("Message") = "Your transaction completed however payment is currently pending. Your order will be shipped once payment is verified."
			redirectPage = "demoOrderComplete.aspx"
	
		Else
		
			Session("Message") = "PayPal payment was unable to complete. Please provide another form of payment to complete your transaction."
			redirectPage = "demoCheckout.aspx"

		End If
		
	'=====================================================================================
	' Determine if the PayPal Express Checkout result was Successful or Error
	'
	' If the Authentication result (PAResStatus) is Y then 
	' the Payer Authentication was successful. The Authorization Message should be processed, 
	' and the User taken to an Order Completion location.
	'
	' If the Authentication results were not successful, the Authentication results were P, X or
	' the ErrorNo was NOT '0' then the Consumer should be redirected, and prompted for another 
	' form of payment.
	'
	' Note that it is also important that you account for cases when your flow logic can account 
	' for error cases, and the flow can be broken after 'N' number of attempts
	'
	'=====================================================================================					
	
	ElseIf Session("Centinel_TransactionType") = "X" Then

		If Session("Centinel_PAResStatus") = "Y" Then
		
			redirectPage = "demoConfirmation.aspx"
		
		Else
		
			Session("Message") = "PayPal express checkout payment was unable to complete. Please provide another form of payment to complete your transaction."
			redirectPage = "demoCheckout.aspx"
		
		End If
		
	'=====================================================================================
	' Determine if the SECURE-eBill result was Successful or Error
	'
	' If the Authentication result (PAResStatus) is P then
	' the Payer Authentication was successful. The Authorization Message should be processed, 
	' and the User taken to an Order Completion location.
	'
	' If the Authentication results were not successful, the Authentication result is X or
	' the ErrorNo was NOT '0' then the Consumer should be redirected, and prompted for another 
	' form of payment.
	'
	' Note that it is also important that you account for cases when your flow logic can account 
	' for error cases, and the flow can be broken after 'N' number of attempts
	'
	'=====================================================================================		
	
	ElseIf Session("Centinel_TransactionType") = "E" Then
	
		If Session("Centinel_PAResStatus") = "P" Then
		
			Session("Message") = "Your transaction completed however payment is currently pending. Your order will be shipped once payment is verified."
			redirectPage = "demoOrderComplete.aspx"
			
		ElseIf Session("Centinel_PAResStatus") = "X" Then
		
			Session("Message") = "Your transaction was canceled prior to completion. Please provide another form of payment."	
			redirectPage = "demoCheckout.aspx"
					
		End If

	
	'=====================================================================================
	' Determine if the Bill Me Later result was Successful, Canceled, or a Request to update billing info was made.
	'
	' If the Authentication result (PAResStatus) is Y then
	' the BML applicatino was successful. The Authorization Message should be processed, 
	' and the User taken to an Order Completion location.
	'
	' If the Authentication results were not successful, the Authentication result is X, or
	' the ErrorNo was NOT '0' then the Consumer should be redirected, and prompted for another 
	' form of payment.
	'
	' If the Authentication result is D then the customer should be redirected to a page where
	' they can update their billing information and then be redirected back to the BML application
	' form.
	'
	' Note that it is also important that you account for cases when your flow logic can account 
	' for error cases, and the flow can be broken after 'N' number of attempts.
	'
	'=====================================================================================		
	
	ElseIf Session("Centinel_TransactionType") = "B" Then
	
		If Session("Centinel_PAResStatus") = "Y" Then
		
			redirectPage = "demoAuthorize.aspx"
			
		ElseIf Session("Centinel_PAResStatus") = "X" Then
		
			Session("Message") = "Your transaction was canceled prior to completion. Please provide another form of payment."	
			redirectPage = "demoCheckout.aspx"

		ElseIf Session("Centinel_PAResStatus") = "D" Then
		
			Session("Message") = "Please update your billing information and submit your order."	
			redirectPage = "demoCheckout.aspx"
					
		End If


	'=====================================================================================
	' Determine if the Authentication result was Successful or Error
	'
	' If the Authentication results (PAResStatus) is a Y or A then 
	' the Payer Authentication was successful. The Authorization Message should be processed, 
	' and the User taken to an Order Completion location.
	'
	' If the Authentication results were not successful, the Authentication results were N, U or
	' the ErrorNo was NOT '0' then the Consumer should be redirected, and prompted for another 
	' form of payment.
	'
	' Note that it is also important that you account for cases when your flow logic can account 
	' for error cases, and the flow can be broken after 'N' number of attempts
	'
	'=====================================================================================

	ElseIf Session("Centinel_TransactionType") = "C" Then
	
		If Session("Centinel_PAResStatus") = "Y" OR Session("Centinel_PAResStatus") = "A" Then
		
			Session("Message") = "Your transaction completed successfully. Your order will be shipped immediately."
			redirectPage = "demoOrderComplete.aspx"
			
		Else
		
			Session("Message") = "Your transaction could not complete. Please provide another form of payment."	
			redirectPage = "demoCheckout.aspx"	
			
		End If

	Else
	
			Session("Message") = "Your order completed but is pending review. Your order will be shipped once payment is verified."	
			redirectPage = "demoOrderComplete.aspx"
	
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
<Title>Processing Your Order</Title>
</HEAD>
<body onLoad="onLoadHandler();">
<FORM name="frmResultsPage" method="Post" action="<%=redirectPage%>" target="_parent">
<noscript>
	<br><br>
	<center>
	<font color="red">
	<h1>Processing Your Transaction</h1>
	<h2>JavaScript is currently disabled or is not supported by your browser.<br></h2>
	<h3>Please click Submit to continue	the processing of your transaction.</h3>
	</font>
	<input type="submit" value="Submit">
	</center>
</noscript>
</FORM>
</BODY>
</HTML>
