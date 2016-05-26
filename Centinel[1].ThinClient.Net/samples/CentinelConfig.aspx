
<%'======================================================================================
'=  Cardinal Commerce (http://www.cardinalcommerce.com)
'=  CentinelConfig.aspx
'=  Version 6.0 03/21/2006
'=	Usage
'=
'=	This configuration file centralizes all Centinel related configurables.
'=	These values are required to be defined to enable the samples for work properly.
'=
'=	Note 
'=
'=      Transaction Testing URL : https://centineltest.cardinalcommerce.com/maps/txns.asp
'=
'=      Your Production Transaction URL, Processor Id, and Merchant Id were assigned to you
'=      upon registration for the Cardinal Centinel service.
'=
'=      Term URL is the fully qualified URL to the ccVerifier.aspx file that is provided
'=		in theses samples.
'======================================================================================%>
<%
   dim MessageVersion, TransactionUrl, MerchantId, ProcessorId, TermURL, TransactionPwd, Timeout
   dim AuthenticationMessaging, SecureEBillMessaging, BMLMessaging
   dim MerchantLogo, VbVLogo, MCSCLogo, JCBLogo, SEBLogo, BMLLogo

   TransactionUrl	= "https://centineltest.cardinalcommerce.com/maps/txns.asp"
    ProcessorId = "202"
    MerchantId = "40158"
   
   TermURL		= "http://yourserver.com/youralias/ccVerifier.aspx"
   MessageVersion	= "1.7"
   TransactionPwd	= "razatest"
   Timeout		= 10000

   AuthenticationMessaging = "<b>For your security, please fill out the form below to complete your order.</b><br/>Do not click the refresh or back button or this transaction may be interrupted or cancelled."
   SecureEBillMessaging = "<b>Complete the confirmation section of the SECURE-eBill below to complete your order.</b><br/>Do not click the refresh or back button or this transaction may be interrupted or cancelled."
   BMLMessaging = "<b>Please complete the Bill Me Later Account Application form below to complete your order.</b><br/>Do not click the refresh or back button or this transaction may be interrupted or cancelled."

   MerchantLogo = "../images/merchant_logo.gif"
   VbVLogo	= "../images/logo_vbv_frame.gif"
   MCSCLogo	= "../images/logo_mcsc_frame.gif"
   BMLLogo	= "../images/logo_bml_frame.jpg"
   JCBLogo	= "../images/logo_jcb_frame.gif"
   SEBLogo	= "../images/logo_seb_frame.gif"
%>
