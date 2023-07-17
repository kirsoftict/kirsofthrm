<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmdatasaveonly.aspx.vb" Inherits="frmdatasaveonly" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head >
    <title></title>
    <link rel="stylesheet" href="jqq/themes/base/jquery.ui.all.css" />

<link rel="stylesheet" href="jq/themes/ui-lightness/jquery.ui.all.css" />
	<script src="jqq/jquery-1.9.1.js"></script>
	<script src="jqq/ui/jquery.ui.core.js"></script>
	<script src="jqq/ui/jquery.ui.widget.js"></script>
	<script src="jqq/ui/jquery.ui.position.js"></script>
	<script src="jqq/ui/jquery.ui.menu.js"></script>
	<script src="jqq/ui/jquery.ui.autocomplete.js"></script>
	<script src="jqq/ui/jquery.ui.progressbar.js"></script>
	<script src="jqq/ui/jquery.ui.datepicker.js"></script>
	<script src="jqq/ui/jquery.ui.button.js"></script>
	<script src="jqq/ui/jquery.ui.dialog.js"></script>
	
	<link rel="stylesheet" href="jq/demos.css">
	<script type="text/javascript" src="scripts/form.js"></script>
</head>
<body>
<form id='frmx' name='frmx' method="post"></form>
    <form id="form1" runat="server">
    <div>
    <asp:Literal runat="server" ID="outp"></asp:Literal>
    </div>
    </form>
</body>
</html>
