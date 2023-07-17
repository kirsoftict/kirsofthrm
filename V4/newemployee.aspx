<%@ Page Language="VB" AutoEventWireup="false" CodeFile="newemployee.aspx.vb" Inherits="newemployee" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <link href="css/style.css" rel="stylesheet" type="text/css"/>
        <link href="css/layout.css" rel="stylesheet" type="text/css"/>
	<link href="css/message.css" rel="stylesheet" type="text/css"/>

	<script type="text/javascript" src="scripts/archive.js"></script>
	
	<script type="text/javascript" src="scripts/archive.js"></script>
    <script type="text/javascript" src="scripts/jquery/jquery.js"></script>
    <script type="text/javascript" src="scripts/jquery/jquery.validate.js"></script>
    <script type="text/javascript" src="scripts/jquery/jquery.form.js"></script>
    <script type="text/javascript" src="scripts/jquery/jquery.tablesorter.js"></script>
    <script type="text/javascript" src="js/validate.js"></script>

    <script type="text/javascript" src="js/core.js"></script>
<script type="text/javascript" src="js/admin.js"></script>
<script type="text/javascript" src="js/validate.js"></script>
    <link rel="stylesheet" type="text/css" media="screen" href="css/main.css" />
<link rel="stylesheet" type="text/css" media="screen" href="css/compstruct.css" />
  
</head>
<body>
<link rel="stylesheet" type="text/css" media="screen" href="oplagin/PimPlugin/css/addEmployeeSuccess.css" />
<script type="text/javascript" src="oplagin/PimPlugin/js/addEmployeeSuccess.js" ></script>
<script type="text/javascript">
    //<![CDATA[
    //we write javascript related stuff here, but if the logic gets lengthy should use a seperate js file
    var edit = "Edit";
    var save = "Save";
    var lang_firstNameRequired = 'Required';
    var lang_lastNameRequired = 'Required';
    var lang_userNameRequired = "Should have at least 5 characters";
    var lang_passwordRequired = "Should have at least 4 characters";
    var lang_unMatchingPassword = "Passwords do not match";
    var lang_statusRequired = "Required";
    var lang_locationRequired = "Required";
    var cancelNavigateUrl = "index.php?menu_no_top=hr";
    var createUserAccount = "0";
    var ldapInstalled = false;
    var fileHelpText = "Accepts jpg, .png, .gif up to 1MB";
    //]]>
</script>
<div id="messagebar" class="" style="margin-left: 16px;width: 700px;">
    <span style="font-weight: bold;"></span>
</div>
<div class="outerbox" style="">
    <div class="mainHeading"><h2>Add Employee</h2></div>
    <div>
        <form id="frmAddEmp" method="post" action="" enctype="multipart/form-data" style="width: auto">
            <table id="addEmployeeTbl"><tr>
  <td><label for="firstName">Full Name</label></td>
  <td colspan=""><input class="formInputText" maxlength="30" type="text" name="firstName" id="firstName" /></td>


  <td></td>
  <td colspan=""><input class="formInputText" maxlength="30" type="text" name="middleName" id="middleName" /></td>


  <td></td>
  <td colspan=""><input class="formInputText" maxlength="30" type="text" name="lastName" id="lastName" /></td>


  <td></td>
  <td colspan=""><div id="empty"></div></td>
</tr>
<tr>
  <td><label for="fullNameLabel"> </label></td>
  <td colspan=""><div id="fullNameLabel"></div></td>


  <td><label for="firstNameLabel"><span class="helpText">First Name</span><span class="required">*</span></label></td>
  <td colspan=""><div id="firstNameLabel"></div></td>


  <td><label for="middleNameLabel"><span class="helpText">Middle Name</span></label></td>
  <td colspan=""><div id="middleNameLabel"></div></td>


  <td><label for="lastNameLabel"><span class="helpText">Last Name</span><span class="required">*</span></label></td>
  <td colspan=""><div id="lastNameLabel"></div></td>
</tr>
<tr>
  <td><label for="employeeId">Employee Id</label></td>
  <td colspan="3"><input class="formInputText" maxlength="10" type="text" name="employeeId" value="0003" id="employeeId" /></td>
</tr>
<tr>
  <td><label for="photofile">Photograph</label></td>
  <td colspan="3"><input class="duplexBox" type="file" name="photofile" id="photofile" /></td>
</tr>
<tr>
  <td><label for="chkLogin">Create Login Details</label></td>
  <td colspan="3"><input style="vertical-align:top" type="checkbox" name="chkLogin" value="1" id="chkLogin" /></td>
</tr>
<tr>
  <td><label for="lineSeperator"><div class="hrLine" id="lineSeperator">&nbsp;</div></label></td>
  <td colspan="3"><div id="Div1"></div></td>
</tr>
<tr>
  <td><label for="user_name">User Name<span class="required">*</span></label></td>
  <td colspan=""><input class="formInputText" maxlength="20" type="text" name="user_name" id="user_name" /></td>


  <td><label for="status">Status<span class="required">*</span></label></td>
  <td colspan=""><select class="formInputText" br="1" name="status" id="status">
<option value="Enabled">Enabled</option>
<option value="Disabled">Disabled</option>
</select></td>
</tr>
<tr>
  <td><label for="user_password">Password<span id="password_required" class="required">*</span></label></td>
  <td colspan=""><input class="formInputText passwordRequired" maxlength="20" type="password" name="user_password" id="user_password" /></td>


  <td><label for="re_password">Confirm Password<span id="rePassword_required" class="required">*</span></label></td>
  <td colspan=""><input class="formInputText passwordRequired" maxlength="20" type="password" name="re_password" id="re_password" /><input type="hidden" name="empNumber" value="0003" id="empNumber" />
<input type="hidden" name="_csrf_token" value="200ba580f7f8deef19a8c5923f31c2a1" id="csrf_token" /></td>
</tr>
 </table>                       
            <div class="formbuttons">
                <input type="button" class="savebutton" id="btnSave" value="Save"  />
                <input type="button" class="savebutton" id="btnCancel" value="Cancel" />
            </div>
        </form>
    </div>
</div>
<div class="paddingLeftRequired"><span class="required">*</span> required field</div>
    	<script type="text/javascript">
//<![CDATA[	    

    	if (document.getElementById && document.createElement) {
	 			//roundBorder('outerbox');
			}
//]]>	
	</script>    
</body>
</html>
