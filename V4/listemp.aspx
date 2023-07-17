<%@ Page Language="vB" CodeFile="listemp.aspx.vb" AutoEventWireup="false" Inherits="listemp" %>
<%@ Import Namespace="kirsoft.hrm" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">

<head><title></title>
	<link rel="stylesheet" href="jq/themes/ui-lightness/jquery.ui.all.css">
	<script type="text/javascript" src="jq/jquery-1.7.2.js"></script>
	<script type="text/javascript" src="jq/ui/jquery.ui.core.js"></script>
	<script type="text/javascript" src="jq/ui/jquery.ui.widget.js"></script>
	<script type="text/javascript" src="jq/ui/jquery.ui.datepicker.js"></script>
	
<script language="javascript" type="text/javascript">
    function orderpass(id)
    {
        //alert();
        //window.location="empcontener.aspx?id=" + id.toString();
        $("#datatake").val(id);
        $("body").css({'visibility':'hidden'});
        $("#frmemplist").submit();

    }
</script></head>

<body>
    
    <%  Dim mk As New formMaker
        Dim row As String
        If Request.QueryString("sql") = "" Then
            
            row = mk.jmakelist22("select * from vwemp_emp_dep_qual order by first_name,active desc, hr", Session("con"))
        Else
            row = mk.jmakelist22(Request.QueryString("sql") & " order by first_name,active desc, hr", Session("con"))
        End If
        Response.Write(row)
        %>
       </body></html>