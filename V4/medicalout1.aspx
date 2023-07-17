<%@ Page Language="VB" AutoEventWireup="false" CodeFile="medicalout1.aspx.vb" Inherits="medicalout1" %>
<%@ Import Namespace="kirsoft.hrm" %>
<%@ Import Namespace="system.data" %>
<%@ Import namespace="System.Data.sqlclient"%>
<% 
    
    If IsError(Session("emptid")) Then
        Session("emp_id") = ""
        Session("emptid") = ""
    ElseIf Request.QueryString("sub") <> "" Then
        
        Session("emp_id") = ""
        Session("emptid") = ""
        
    End If
   
    Dim keyp As String = ""
    Dim paths, loc As String
    Dim fl As New file_list
    Dim fm As New formMaker
    If Request.QueryString("dox") = "edit" Then
        keyp = "update"
    ElseIf Request.QueryString("dox") = "delete" Then
        keyp = "delete"
    ElseIf Request.QueryString("dox") = "upload" Then
    Else
        keyp = "save"
    End If
    Dim idx As String = ""
    idx = Request.QueryString("id")
    Dim msg As String = ""
    Dim dbx As New dbclass
   
      Dim sql As String = ""
      Dim flg As Integer = 0
      Dim flg2 As Integer = 0
      ' Response.Write(sc.d_encryption("zewde@123"))
      Dim rd As String = ""
     
      Dim tbl As String = ""
      Dim key As String = ""
        Dim keyval As String = ""
        tbl = Request.QueryString("tbl")
        key = Request.QueryString("key")
        rd = Request.QueryString("rd")
        If Request.QueryString.HasKeys = True Then
            If Request.QueryString("dox") = "" Then
                keyval = Request.QueryString("keyval")
                If Request.QueryString("task") = "update" Then
                ' Response.Write("<script type='text/javascript'>alert('updating....');</script>")
                    sql = dbx.makeupdate_statement(tbl, Request.QueryString, session("con"), key, keyval)
                    flg = dbx.save(sql, session("con"),session("path"))
                    ' Response.Write(sql)
                    If flg = 1 Then
                    msg = "Data Updated"
                    End If
                ElseIf Request.QueryString("task") = "delete" Then
                'Response.Write("<script type='text/javascript'>alert('deleting....');</script>")
                    ' sql = dbx.makeupdate_statement(tbl, Request.QueryString, session("con"), key, keyval)
                    sql = "delete from " & Request.QueryString("tbl") & " where id=" & Request.QueryString("id") '
                    flg = dbx.save(sql, session("con"),session("path"))
                   
                    ' Response.Write(sql)
                    If flg = 1 Then
                    msg = "Data deleted"
                End If
                ElseIf Request.QueryString("task") = "save" Then
                sql = dbx.makest(tbl, Request.QueryString, session("con"), key)
                'Response.Write(sql)
                flg = dbx.save(sql, session("con"),session("path"))
                If flg = 1 Then
                    msg = "Data Saved"
                End If
          
               
                ' Response.Write(sql)
               
           
            End If
            If flg <> 1 Then
                'Response.Write("<span style='font-size:15pt; color:Red;'>Sorry Data doesnt change </span>")
            End If
          
   
        End If
    End If
    If Request.QueryString("task") = "deletefile" Then
        
                
        If Request.QueryString("id") <> "" Then
                    
            paths = Request.QueryString("id")
            paths = paths.Replace("~", "\")
            msg = fl.deletefile(paths)
        End If
        flg = 1
    End If
    If Request.Form("vname") <> "" Then
        Dim nm() As String
        Dim wr As String = ""
        
        nm = Request.Form("vname").Split(" ")
        If nm.Length > 0 Then
            
            For i As Integer = 0 To nm.Length - 1
                If i = 0 Then
                    wr &= " where first_name='" & nm(i) & "'"
                ElseIf i = 1 Then
                    wr &= " and middle_name='" & nm(i) & "'"
                ElseIf i = 2 Then
                    wr &= " and last_name='" & nm(i) & "'"
                End If
            Next
        End If
        If wr <> "" Then
            Dim rss As DataTableReader
            Dim ccc As String
            ccc = fm.getinfo2("select count(id) as no from emp_static_info " & wr, Session("con"))
            
            If IsNumeric(ccc) = True Then
                If CInt(ccc) = 1 Then
                    ccc = fm.getinfo2("select emp_id as no from emp_static_info " & wr, Session("con"))
                    Session("emp_id") = ccc
                    rss = dbx.dtmake("temp", "Select id from emprec where emp_id='" & ccc & "' and active='y'", Session("con"))
                    If rss.HasRows = True Then
                        rss.Read()
                        Session("emptid") = (rss.Item(0)).ToString
                    End If
                    rss.Close()
                End If
            End If
            
        End If
    End If
    paths = ""
    loc = ""
    loc = Request.ServerVariables("url").Substring(1, Request.ServerVariables("url").Length - 1)
    Dim namelist As String = ""
     	   
    namelist = fm.getjavalist2("emp_static_info", "first_name,middle_name,last_name", Session("con"), " ")
   
    Dim dbs As New dbclass
    Dim rs As DataTableReader
    Dim bal As Double = 0
    Dim addnew As String = ""
    Dim used As Double = 0
    Dim bgt As Double = 0
    Dim bgtid As String = ""
    Dim bgfrom, bgto As Date
    If Request.QueryString("task") = "closebgt" Then
      
        bgtid = fm.getinfo2("select id from emp_medical_all where emptid='" & Session("emptid") & "' and active='y' order by id desc", Session("con"))
        If bgtid > 0 Then
            sql = "update emp_medical_all set active='n' where id=" & bgtid.ToString
            Dim rtn As String
            rtn = dbs.excutes(sql, Session("con"),session("path"))
            ' Response.Write(rtn)
        End If
    End If
    sql = "select * from emp_medical_all where emptid='" & Session("emptid") & "' and active='y' order by id desc"
    rs = dbs.dtmake("emp_mmm", sql, Session("con"))
    If rs.HasRows Then
    
        rs.Read()
        bgtid = rs.Item("id")
        bgfrom = CDate(rs.Item("date_from")).ToShortDateString
        bgto = CDate(rs.Item("date_exp")).ToShortDateString
        sql = "select sum(amt_used) as bal from emp_medical_take where m_id=" & rs.Item("id") & " Group by m_id"
        Dim rs2 As DataTableReader = dbs.dtmake("empmk", sql, Session("con"))
        If rs2.HasRows Then
            rs2.Read()
            used = rs2.Item("bal")
        Else
            used = 0
        End If
        bgt = rs.Item("mallwance")
        bal = bgt - used
        rs2.Close()
        rs2 = Nothing
    Else
        addnew = "on"
    End If
    rs.Close()
   
    
 %> 
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title></title>
<link rel="stylesheet" href="jqq/themes/base/jquery.ui.all.css" />

<link rel="stylesheet" href="jq/themes/ui-lightness/jquery.ui.all.css" />
<script src="jqq/jquery-1.9.1.js"></script>
	<script src="jqq/ui/jquery.ui.core.js"></script>
	<script src="jqq/ui/jquery.ui.widget.js"></script>
	<script src="jqq/ui/jquery.ui.position.js"></script>
	<script src="jqq/ui/jquery.ui.menu.js"></script>
	<script src="jqq/ui/jquery.ui.autocomplete.js"></script>
    <script type="text/javascript" src="jqq/ui/jquery.ui.datepicker.js"></script>
<script type="text/javascript" src="scripts/form.js"></script>
 <script src="jqq/ui/jquery.ui.button.js"></script>

    <script src="jqq/ui/jquery.ui.dialog.js"></script>
	
	<link rel="stylesheet" href="jq/demos.css">


<link rel="stylesheet" type="text/css" media="screen" href="css/kir.login.css" /> 
<script src="scripts/kirsoft.java.js" type="text/javascript"></script>
<script src="scripts/script.js" type="text/javascript"></script>


   
	<script src="scripts/kirsoft.required.js" type="text/javascript"></script>
<script type="text/javascript">
    var prv;
    prv = "";
    var id;
    var focused = "";
    var requf = ["m_id", "amt_used", "used_date", "x"];
    var fieldlist = ["m_id", "amt_used", "used_date", "who_reg", "date_reg", "x"];
    function validation1() {
        if ($('#m_id').val() == '') { showMessage('m_id cannot be empty', 'm_id'); $('#m_id').focus(); return false; }
        if ($('#amt_used').val() == '') { showMessage('amt_used cannot be empty', 'amt_used'); $('#amt_used').focus(); return false; }
        if ($('#used_date').val() == '') { showMessage('used_date cannot be empty', 'used_date'); $('#used_date').focus(); return false; }
        if (parseFloat($('#amt_used').val())>parseFloat(<% response.write(bal) %>)){showMessage('The balance is less than the request amount','amt_used');$('#amt_used').focus(); return false;}
        else if (focused == "") {
            var ans;
            ans = checkblur();
            if (ans != true) {
                $("#" + ans).focus();
            } else {
                var str = $("#frmemp_medical_take").formSerialize();
                $("#frmemp_medical_take").attr("action", "?tbl=emp_medical_take&task=<% response.write(keyp) %>&lrd=empcontener&key=id&keyval=<% response.write(idx) %>&rd=empcontener.aspx&" + str);
                $("#frmemp_medical_take").submit();
                return true;
            }
        }
    } 
     var namelist=[<% response.write(namelist) %>];</script>
<script type="text/javascript">

function goupload(whr,fn)
{

    var ftype=".doc,.docx,.pdf,.jpg";
    var size=1000000;
   <% if  String.IsNullOrEmpty(session("emp_id"))=false then 
   response.write( "var loct='" & session("emp_id").trim & "/medical/' +fn+ '/';")
   end if%>
  var hw= window.screen.availHeight;
  var ww=window.screen.availWidth;
   // alert(whr + fn.toString());
     $('#frmx').attr("target","uploadf");
       
       $('#frmx').attr("action","allupload.aspx?upload=phase1&loc=<%response.write(loc) %>&tar=frm_tar&ftype="+ftype+"&size="+size+"&loct="+loct);
     //alert("allupload.aspx?upload=phase1&ftype="+ftype+"&size="+size+"&loct="+loct);
      
       $('#upload').css({top:'0px',left:'0px'});
       showobj("upload");
       $('#frmx').submit();
 // $( '#upload').dialog('destroy');
//$( '#upload' ).dialog({resizable: true,modal: true});
  //   $('#upload').css({'visibility':'visible','display':'inline'});
}
function addnew()
{
    

     $('#frmx').attr("target","fpay");
      $("#fpay").attr("frameborder","0");
      
       $('#frmx').attr("action","empmedicalbgt.aspx?loc=<%response.write(loc) %>&tar=frm_tar");
       $('#frmx').submit();
      // alert("empmedicalbgt.aspx?loc=<%response.write(loc) %>&tar=frm_tar");
      // $('#post').attr("disabled","disabled");
    $('#pay').css({top:'0px',left:'0px'});
     $("#pay").remove("display");
            $("#pay").dialog({
            title:'Add Medical Budget',
                    height:300,
                    width:600,
                    modal:true});
    //alert(whr + fn.toString());
      // $( '#upload').dialog('destroy');
//$( '#upload' ).dialog({resizable: true,modal: true});
  //   $('#upload').css({'visibility':'visible','display':'inline'});
}
function addnewup()
{

     $('#frmx').attr("target","fpay");
      $("#fpay").attr("frameborder","0");
      
       $('#frmx').attr("action","empmedicalbgt.aspx?loc=<%response.write(loc) %>&tar=frm_tar&update=onx");
       $('#frmx').submit();
       $('#post').attr("disabled","disabled");
    $('#pay').css({top:'0px',left:'0px'});
     $("#pay").remove("display");
            $("#pay").dialog({
            title:'Edit Medical Budget',
                    height:300,
                    width:600,
                    modal:true});
   // alert(whr + fn.toString());
      // $( '#upload').dialog('destroy');
//$( '#upload' ).dialog({resizable: true,modal: true});
  //   $('#upload').css({'visibility':'visible','display':'inline'});
}
function delfile(val,ans)
       { 
       ans = confirm("are u sure want delete");
      // alert('<% response.write(loc) %>');
        if( ans==true)
          { // alert(val);
               // $('#frmx').attr("target","frm_tar");
                $('#frmx').attr("action","?task=deletefile&id="+val+"&tbl=file");
                $('#frmx').submit();
            }
        
            
       }
       function approved(val,ans)
       { 
         //$('#approvedx').load("leavecalc.aspx?id="+val+"&task="+ans+"&tbl=emp_medical_take");
       
             $('#frmx').attr("target","appf");
       
       $('#frmx').attr("action","leavecalc.aspx?id=" + val + "&task=" +ans);
       $('#frmx').submit();
       $('#approved').css({top:'0px',left:'0px',width:'1100px',height:'400px',background:'#ffffff'});
       showobj("approved");
       }
        </script>
         <script type="text/javascript">
             function delxx(val, ans, hd) {

                 if (ans == "yes") 
                 {
                     $('#frmx').attr("target", "_self");
                     $('#frmx').attr("action", "<% response.write(loc) %>?task=delete&id=" + val + "&tbl=emp_medical_take");
                     $('#frmx').submit();
                 }
                 else {
                     //ha(hd);
                 }
             }
   </script>
<script type="text/javascript" language="javascript">
    function closeit() { 
     $('#frmx').attr("target","frm_tar");
                $('#frmx').attr("action","?task=closebgt");
                $('#frmx').submit();
    }
    function print(loc, title, head, footer) {
        var printFriendly = document.getElementById(loc)
        var printWin = window.open("about:blank", title, "menubar=no;status=no;toolbar=no;");
        printWin.document.write("<html><head><title>company name</title></head><body><h1>" + head + "</h1>" + printFriendly.innerHTML + "<label class='smalllbl'>" + footer + "</label></body></html>");
        printWin.document.close();
        printWin.window.print();
        printWin.close();
    }
   
    </script>
</head>

<body style="height:auto;">


<form id='frmsbyname' name='frmsbyname' method='post' action='medicalout1.aspx?msg='>
    <table cellspacing='0px' cellpadding='0px'><tr><td>
    <input type='text' name='vname' id='vname' style='font-size:9pt;'  onkeyup="javascript:startwith('vname',namelist);" /><br/>
    <label class='lblsmall'>write name</label></td>
    <td valign='top'><input type='button' class='searchx' id='searchx' value='Go' name='searchx' onclick="document.frmsbyname.submit()" />
    </td></tr></table></form>
<%  ' Dim fl As New file_list
    Response.Write(fl.msgboxt("upload", "Upload", "<iframe name='uploadf' id='uploadf' width='500' height='250' frameborder='0' src='' scrolling='no'></iframe>"))
    Response.Write(fm.popupwindow("approved", "Approved", "<iframe name='appf' id='appf' width='1200' height='1000' frameborder='0' src='' scrolling='yes'></iframe>"))
    Response.Write(fl.dialog("pay", "", "<iframe name='fpay' id='fpay' width='500' height='500' frameborder='0' src='' scrolling='no'></iframe>"))%>

<div id="iframeh" style="visibility:hidden; width:300px; height:100px; overflow:hidden; display:none;" >
</div>
<div id='helpx'> </div>
<% ' 'Response.Write(fm.helpback(False, True, False, False, "", ""))%>

    <% If Session("username") = "" Then
       %>
       <script type="text/javascript">
           //document.location.href="admin_home.php"
           window.location = "logout.aspx";
</script>
       <%
       End If
       
   


'Response.Write(Session("fullempname"))

Dim sc As New k_security
' Response.Write(sc.d_encryption("zewde@123"))
If Request.Form.HasKeys = True Then
    'Dim db As New dbclass
    ' Dim sql As String
    ' sql = db.makest("tblworkexp", Request.Form, session("con"), "")
    'Response.Write(sql)
    'db.save(sql, session("con"),session("path"))
End If
       For Each k As String In Request.ServerVariables
           'Response.Write(k & "===>" & Request.ServerVariables(k) & "<br>")
       Next
'Response.Write("<br />" & Request.Form("do"))
      
 %>
 
<script language="javascript">
    var bc=<% response.write(bal.tostring) %>
</script>
<div style="font-size:10pt;">
<% 
    If IsError(Session("emptid")) = False Then
        If String.IsNullOrEmpty(Session("emptid"))=False Then
            If addnew = "on" Then
                %>
                <div><span onclick="javascript:addnew();" style="color:Blue;cursor:pointer;">Add Budget</span></div>
                <%
            End If
            If Request.Form("vname") <> "" Then
                Response.Write(Request.Form("vname"))
            Else
                Response.Write(fm.getfullname(Session("emp_id"), Session("con")))
                End If
                If addnew = "" Then
                    If String.IsNullOrEmpty(bgfrom.ToString) = False Then
                        Response.Write("<br> Budget From:" & MonthName(bgfrom.Month) & " " & bgfrom.Day.ToString & ", " & bgfrom.Year.ToString & " - " & MonthName(bgto.Month) & " " & bgto.Day.ToString & ", " & bgto.Year.ToString)
                        %>
                                        <div><span onclick="javascript:closeit();" style="color:Blue;cursor:pointer;">Close Budget</span>&nbsp;&nbsp;&nbsp;&nbsp;|&nbsp;&nbsp;&nbsp;&nbsp;<span onclick="javascript:addnewup();" style="color:Blue;cursor:pointer;">Update Budget</span></div>

                        <%
                    End If
                End If
                %>
                </div>
 <div id="balshow">
 <table cellpadding="7">
 <tr><td>Amount Budget</td><td align="right"><% Response.Write(bgt.ToString)%></td></tr>
  <tr><td>Total Used Amount</td><td align="right"><% Response.Write(used.ToString)%></td></tr>
   <tr><td><b>Balance</b></td><td align="right"><b><% Response.Write(bal.ToString)%></b></td></tr>
   
 </table>
 <hr />
 </div>
 
<div id="approvedx"></div>
 <div id="formouterbox_small">
     <div id="formheader">
    <span class="titlet">
This is Title</span>
<span class="close" id="clickclose_s" style="cursor:pointer;"></span>
        <div class="head1">&nbsp;</div><div class="head2">&nbsp;</div><div class="head3">&nbsp;</div>
        </div>
    <div id="forminner">
    <span id="messagebox"></span>
    <form method='post' id='frmemp_medical_take' name='frmemp_medical_take'> 
<table><tr><td>Medical Bgt Id.<sup style='color:red;'></sup></td><td>:</td><td><% response.write(bgtid.tostring) %><input type='hidden' id='m_id' name='m_id' value='<% response.write(bgtid.tostring) %>' /><br /><label class='lblsmall'></label></td>
</tr><tr><td>Amount Used<sup style='color:red;'>*</sup></td><td>:</td><td><input type='text' id='amt_used' name='amt_used' value='' /><br /><label class='lblsmall'></label></td>
</tr><tr>
<td>Date Paid<sup style='color:red;'>*</sup></td><td>:</td><td>
<input type='text' id='used_date' name='used_date' value='' /><br /><label class='lblsmall'></label>
<script language='javascript' type='text/javascript'>    $(function () { $("#used_date").datepicker({ changeMonth: true, changeYear: true }); $("#used_date").datepicker("option", "dateFormat", "mm/dd/yy"); });</script>
<input type='hidden' id='who_reg' name='who_reg' value='<% response.write(session("emp_iid")) %>' />
<input type='hidden' id='date_reg' name='date_reg' value="<% dim lucur(3) as string
 lucur(2) = Today.Year.ToString
     lucur(1) = Today.Month.ToString
     lucur(0) = Today.Day.ToString
 dim sdate as string=lucur(1) & "/" & lucur(0) & "/" & lucur(2)
 response.write(sdate) %>" /></td>
</tr><tr><td colspan='4'>
<% If bal > 0 Then%>
<input type='button' name='btnSave' id='btnSave' value='Save' />
<% Else%>
<input type='button' name='btnSave' id='Button1' value='Save' disabled=disabled />
<% End If%>
<input type='reset' onclick="javascript:$('#btnSave').attr('title','Save');$('#btnSave').attr('value','Save');" /></td></tr></table></form>
    </div>
    <sup style="color:Red;">*</sup>Required Fields
 </div>


   <form id="frmx" name='frmx' action="" method="post">
    </form>
 <div id="listx" style=" width:70%; padding:0px 7px 0px 0px; float:left">
    <%  Dim floc As String
        Dim db As New dbclass
        Dim dt As DataTableReader
        '  Dim loc As String
        If keyp = "update" Then
          
            dt = db.dtmake("new" & Today.ToLocalTime, "select * from emp_medical_take where id=" & Request.QueryString("id"), session("con"))
      If dt.HasRows = True Then
          dt.Read()
          Response.Write("<script type='text/javascript'>")
          For k As Integer = 0 To dt.FieldCount - 4
                    'Response.Write("//" & dt.GetDataTypeName(k).ToLower)
                    If LCase(dt.GetDataTypeName(k)) = "string" And dt.IsDBNull(k) = False Then
                        %>
                    $('#<% Response.Write(dt.GetName(k) & "').val('" & dt.Item(k).trim & "');")%>
                    <% 
                    ElseIf LCase(dt.GetDataTypeName(k)) = "datetime" And dt.IsDBNull(k) = False Then
                        Dim sdatex As Date = dt.Item(k)
                        Dim d As String = sdatex.ToShortDateString
                        Dim da As String = sdatex.Day
                        Dim mm As String = sdatex.Month
                        Dim yy As String = sdatex.Year
                        d = mm & "/" & da & "/" & yy
                        Response.Write("$('#" & dt.GetName(k) & "').val('" & d & "');")
                    Else
                         %>
                    $('#<% Response.Write(dt.GetName(k) & "').val('" & dt.Item(k) & "');")%>
                    <%
                    End If
                   
                Next
                    Response.Write("$('#btnSave').attr('title','update');$('#btnSave').attr('value','Update');</script>")
                End If
                dt.Close()
            End If
            db = Nothing
            dt = Nothing
        Dim mk As New formMaker
        Dim row As String
        floc = Server.MapPath("employee") & "/" & Session("emp_id") & "/medical"
        loc = Request.ServerVariables("url").Substring(1, Request.ServerVariables("url").Length - 1)
        Dim sqlx As String = "select id,m_id,amt_used,used_date from emp_medical_take where m_id='" & bgtid & "' order by id desc"
        row = mk.edit_del_list2("emp_medical_take", sqlx, "Budget id,Amount Used, Date", Session("con"), loc, floc, True, True, True, True)
        Response.Write(row)
        'files uploaded
    
    %>
 </div>
 <% 
     Dim f As New file_list
     loc = Request.ServerVariables("url").Substring(1, Request.ServerVariables("url").Length - 1)
     Response.Write(filesview(Server.MapPath("employee"), Session("emp_id").trim, "medical", "employee"))%>
 <div></div>
 <div style=" float:left; width:30%;">
 <script type="text/javascript">
    function onchangebb()
    {
        var str=$("#upload").formSerialize();
        //alert("jqueryupdate.aspx?empid=<% response.write(session("emp_id")) %>&" + str + "&dest=leavetake&ftype=jpg,gif,png,doc,docx,pdf&task=upload");
       $("#newpage").load("jqueryupdate.aspx?empid=<% response.write(session("emp_id").trim) %>&" + str + "&dest=leavetake&ftype=jpg,gif,png,doc,docx,pdf&what_task=upload");
      }
 </script>

 </div>
 <div id="print1" style=" float:right; width:59px; height:33px; color:Gray;" onclick="javascirpt:print('listx','','','');">print</div>
 <div style=" clear:both;"></div>
<div id="newpage"></div>
<script type="text/javascript" language="javascript">
hform=findh(document.getElementById("middle_bar"));
    $('.titlet').text("Leave Request");
    //showobjar("formx","titlet",22,2);
   $( "#messagebox" ).text("<% response.write(Request.QueryString("msg")) %>");
  </script>
  
    
   <form id="frmup" action="" method="post" enctype="multipart/form-data">
    </form>
   
   <%  ' Response.Write(keyp)
        
       If keyp = "delete" Then
           Dim fs As New file_list
           Dim con As String
           Dim str As String
           con = "<span style='color:red;'> This row of data will not be come again.<br />Are you sure you want delete it?<br /><hr>" & _
           "<img src='images/gif/btn_delete.gif' style='cursor:pointer;' onclick=" & Chr(34) & "javascript:delxx('" & idx & "','yes','del123');" & Chr(34) & "></span>"
           fs.msgboxt("del123", "Caution! Deleting", con)
           str = "<div id='del123' style=" & Chr(34) & "opacity:0.9;filter:alpha(opacity=90); background:#9fdfaf; left:400px; top:200px; width:600px; height:400px; text-align:center; vertical-align:middle; position:absolute; content:open-quote;" & Chr(34) & _
            "><div style=" & Chr(34) & "height:30px; background:url(images/blue_banner-760x147.jpg); vertical-align:top;" & Chr(34) & _
            "><div style=" & Chr(34) & "text-align:left; font-size:16px; color:#000099; width:120px; position:absolute; left:2px;" & Chr(34) & " dir=" & Chr(34) & "ltr" & Chr(34) & _
            "><b>Warrening</b></div><div style=" & Chr(34) & "cursor:pointer; text-align:right; height:30px; width:22px; color:#CC0000; background:url(../images/xp.gif); background-repeat:no-repeat; right:0px; position:absolute" & Chr(34) & " dir=" & Chr(34) & "rtl" & Chr(34) & " onClick=" & Chr(34) & "javascript: document.getElementById('" & CStr(ID) & "').style.visibility='hidden';" & Chr(34) & _
            ">&nbsp; </div></div><br /><br />" & _
       "<div align=" & Chr(34) & "center" & Chr(34) & " style=" & Chr(34) & "width:100%; height:300px; overflow:scroll; font-size:12px; color:blue;" & Chr(34) & _
       ">&nbsp;&nbsp;" & CStr(con) & "</div></div>"
           ' Response.Write(str)
           %> 
           <div id="dialog-modal" title="Caution"><% response.write(con) %></div>
           <script type="text/javascript">

               //$( "#dialog:ui-dialog" ).dialog( "destroy" );

               $("#dialog-modal").dialog({
                   resizable: true,
                   modal: true
               });
		
           </script>
           <%  fs = Nothing
          
       End If
       f = Nothing
       db = Nothing
      
           dt = Nothing
       End If
   End If
   For Each k As String In Request.ServerVariables
       ' Response.Write(k & "===>" & Request.ServerVariables(k) & "<br>")
   Next
   If flg = 1 Then
         %>
    <script type="text/javascript">
        //$(document).delay(80000);
        $('#frmx').attr("target","frm_tar");
        $('#frmx').attr("action","<% response.write(Request.ServerVariables("URL")) %>?msg=<% response.write(msg) %>" );
        $('#frmx').submit();
    </script>
   <%  ElseIf flg = 2 Then
          
       %><script type="text/javascript">
             showMessage('<%response.write(msg) %>', 'date_taken_from');
       </script>
       <%
       End If
       Dim outp As String = ""
       sql = "select * from emp_medical_all where emptid='" & Session("emptid") & "' and active='n' order by id desc"
       rs = dbs.dtmake("emp_mmm", sql, Session("con"))
       If rs.HasRows Then
           Dim amt, sumused As Double
           While rs.Read
               bgtid = rs.Item("id")
               bgfrom = CDate(rs.Item("date_from")).ToShortDateString
               bgto = CDate(rs.Item("date_exp")).ToShortDateString
               bgt = rs.Item("mallwance")
               sql = "select * from emp_medical_take where m_id=" & rs.Item("id") & " order by used_date"
               Dim rs2 As DataTableReader = dbs.dtmake("empmk", sql, Session("con"))
               outp &= "<table><tr style='font-weight:bold;font-size:10pt;border-left:1px solid black;'><td>Budget id:" & bgtid & "</td><td>Budget From:" & MonthName(bgfrom.Month) & " " & bgfrom.Day.ToString & ", " & bgfrom.Year.ToString & " - " & MonthName(bgto.Month) & " " & bgto.Day.ToString & ", " & bgto.Year.ToString & "</td><td style='text-align:right;'>Budget Amount:" & FormatNumber(bgt.ToString, 2, TriState.True, TriState.True, TriState.True).ToString & "</td></tr>"
               outp &= "<tr><td colspan=2><table><tr style='font-weight:bold;font-size:9pt;'><td>Request id</td><td>Date</td><td>Amount</td></tr>"
               sumused = 0
               While rs2.Read
                   amt = 0
                   amt = rs2.Item("amt_used")
                   sumused += amt
                   outp &= "<tr><td>" & rs2.Item("id") & "</td><td>" & rs2.Item("used_date") & "</td><td  style='text-align:right;'>" & FormatNumber(amt.ToString, 2, TriState.True, TriState.True, TriState.True).ToString & "</td></tr>"
   
               End While
               outp &= "<tr ><td>&nbsp;</td><td>&nbsp;</td><td  style='text-align:right;'><b>Total Used:" & FormatNumber(sumused, 2, TriState.True, TriState.True, TriState.True).ToString & "</b></td></tr></table></td></tr><tr style='font-weight:bold'><td colspan=2 style='text-align:right;'>Balance:" & FormatNumber((CDbl(bgt) - sumused), 2, TriState.True, TriState.True, TriState.True).ToString & "</td><td>Status:"
               If rs.Item("active") = "n" Then
                   outp &= "Closed"
               Else
                   outp &= "Active"
               End If
               outp &= "</td></tr></table>"
               rs2.Close()
               rs2 = Nothing
           End While
            
       End If
       rs.Close()
    
       Response.Write(outp)
       %>
   
   
</body>
</html>
<script type="text/javascript">
    $(document).ready(function () {
        $("#amt_used").keyup(function () {
            // alert($("#amt_used").val() + ">" + bc);
            if (parseFloat($("#amt_used").val()) > parseFloat(bc)) {
                $("#amt_used").val(bc);
                alert("Maximum Amount is written down");
               // $("#btnSave").attr("disabled", "disabled");
            }
            else {
                $("#btnSave").removeAttr('disabled');
            }
        });

    });
</script>