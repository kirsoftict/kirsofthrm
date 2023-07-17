<%@ Page Language="VB" AutoEventWireup="false" CodeFile="empot.aspx.vb" Inherits="empot" %>
<%@ Import Namespace="kirsoft.hrm" %>
<%@ Import Namespace="system.data" %>
<%@ Import namespace="System.Data.sqlclient"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
<title></title>
<link rel="stylesheet" href="jq/themes/ui-lightness/jquery.ui.all.css" />
<link rel="stylesheet" href="jqq/themes/base/jquery.ui.all.css">
	<script src="jqq/jquery-1.9.1.js"></script>
	<script src="jqq/ui/jquery.ui.core.js"></script>
	<script src="jqq/ui/jquery.ui.widget.js"></script>
	<script src="jqq/ui/jquery.ui.position.js"></script>
	<script src="jqq/ui/jquery.ui.menu.js"></script>
	<script src="jqq/ui/jquery.ui.autocomplete.js"></script>
	<script src="jqq/ui/jquery.ui.dialog.js"></script>
<script src="jqq/ui/jquery.ui.button.js"></script>
    <script src="jqq/ui/jquery.ui.datepicker.js"></script>
<script type="text/javascript" src="scripts/form.js"></script>

<script src="scripts/kirsoft.java.js" type="text/javascript"></script>
  
	<link rel="stylesheet" type="text/css" media="screen" href="css/kir.login.css" />

	



  <%
   
      Dim keyp As String = ""
      If Request.QueryString("dox") = "edit" Then
          keyp = "update"
      ElseIf Request.QueryString("dox") = "delete" Then
          keyp = "delete"
      Else
          keyp = "save"
      End If
    Dim fm As New formMaker
      Dim emp_id, emptid As String
      Dim arrx() As String = {"Reg", "Nig", "WE", "HD"}
      bee()
      Dim idx As String = ""
      idx = Request.QueryString("id")
      Dim msg As String = ""
      Dim flg As Integer = 0
      Dim flg2 As Integer = 0
      Dim namelist As String = ""
      Dim rd As String = ""

      Dim tbl As String = ""
      Dim key As String = ""
      Dim keyval As String = ""
      tbl = Request.QueryString("tbl")
      key = Request.QueryString("key")
      rd = Request.QueryString("rd")
      namelist = fm.getjavalist2("emp_static_info", "first_name,middle_name,last_name", session("con"), " ")


 %>
 <script type="text/javascript">
 var likeid;
  function showHideSubMenu(link,id) {

        var uldisplay="";
        var newClass="";
        
        
        if (link.className == 'expanded') {

            // Need to hide
            
            uldisplay = 'none';
            newClass = 'collapsed';

        } else {
            // Need to show
            uldisplay = 'block';
            newClass = 'expanded';
        }

       
        $("#"+id).css({'display':  uldisplay});
        link.className = newClass;
    }
    function showHideSubMenux(linkid,id) {

        var uldisplay="";
        var newClass="";
        
        link=document.getElementById(linkid)
        if (link.className == 'expanded') {

            // Need to hide
            
            uldisplay = 'none';
            newClass = 'collapsed';

        } else {
            // Need to show
            uldisplay = 'block';
            newClass = 'expanded';
        }

       
        $("#"+id).css({'display':  uldisplay});
        link.className = newClass;
    }
var prv;
  prv="";
var id;
var focused="";
var requf=["ot_date","time_diff","factored","datepaid","x"];
var fieldlist=["emp_id","emptid","ot_time","ot_end","description","datepaid","who_reg","date_reg","x"];
function validation1(){
if ($('#vname').val() == '') {showMessage('vname cannot be empty','vname');$('#vname').focus();return false;}
if ($('#ot_date').val() == '') {showMessage('OT Date cannot be empty','datework');$('#ot_date').focus();return false;}
if ($('#factored').val() == '') {showMessage('factored cannot be empty','factored');$('#factored').focus();return false;}
else if(focused=="") { var ans;
ans=checkblur();
if(ans!=true){ 
 $("#" + ans).focus();
}else{
   var str=$("#frmemp_ot").formSerialize();
   $("#frmemp_ot").attr("action","?tbl=emp_ot&task=<% response.write(keyp) %>&lrd=empcontener&key=id&keyval=<% response.write(idx) %>&rd=empcontener.aspx&" + str);
    $("#frmemp_ot").submit();
  return true;}
  }
} 
    var namelist=[<% response.write(namelist) %>];
   
		
		
 </script>
 <style type="text/css">
    .expanded img
    {
    	src: url(images/gif/expanded.gif) ;
        
    }

    .collapsed  img{
        src: url(images/gif/collapsed.gif);
       
        
    }
 </style>
</head>

<body style="height:auto;">
<div></div>


 <div id="formouterbox" style="width:800px;">
 <div id='form1'>
     <div id="formheader">
    <span class="titlet">
This is Title</span>
<span class="close" id="clickclose_s" style="cursor:pointer;"></span>
        <div class="head1">&nbsp;</div>
        <div class="head2">&nbsp;</div>
        <div class="head3">&nbsp;</div>
        </div>
    <div id="forminner">
    <span id="messageboxx"></span>

<form method='post' id='frmemp_ot' name='frmemp_ot' action=""> 
<table width="700px"><tr>
<td>Search by Name</td><td>:</td><td colspan='5'>
<input type="hidden" name="id" id="id" />
<input type='text' name='vname' id='vname' style='font-size:9pt;' onkeyup="javascript:startwith('vname',namelist);" /><input type="checkbox" value='hasname' id='takename' name='takename' />Take name to the next Entery</td>
</tr><tr><td><input type='hidden' id='emp_id' name='emp_id' value='<%response.write(session("emp_id")) %>' />
<input type='hidden' id='emptid' name='emptid' value='<% response.write(session("emptid")) %>' />
OT Date<sup style='color:red;'>*</sup></td><td>:</td>
<td><input type='text' id='ot_date' name='ot_date' value='' /><br /><label class='lblsmall'></label></td>
<script language='javascript' type='text/javascript'> $(function() {$( "#ot_date").datepicker({changeMonth: true,changeYear: true	}); $( "#ot_date" ).datepicker( "option","dateFormat","mm/dd/yy");});</script>
<td>OT Paid<sup style='color:red;'>*</sup></td><td>:</td>
<td><input type='text' id='datepaid' name='datepaid' value='' /><br /><label class='lblsmall'></label></td>
<script language='javascript' type='text/javascript'> $(function() {$( "#datepaid").datepicker({changeMonth: true,changeYear: true	}); $( "#datepaid" ).datepicker( "option","dateFormat","mm/dd/yy");});</script>
</tr>

<tr><td colspan="8">
   
    <fieldset style="border:1px gray solid;">
<table style="width:250px;"><tr><td>
  Regular Hrs.</td><td>:</td><td><input type='text' id='Reg' name='Reg' value='' size="2" /></td></tr>
  <tr><td>
  Night Hrs.</td><td>:</td><td><input type='text' id='Nig' name='Nig' value='' size="2" /></td></tr>
  <tr><td>
  Sunday(Weekends) Hrs.</td><td>:</td><td><input type='text' id='WE' name='WE' value='' size="2" /></td></tr>
  <tr><td>
  Public Holiday Hrs.</td><td>:</td><td><input type='text' id='HD' name='HD' value='' size="2" /></td></tr>
</table></fieldset>
</td></tr>
    
<input type='hidden' id='who_reg' name='who_reg' value="<%response.write(session("username")) %>" />
<input type='hidden' id='date_reg' name='date_reg'  value="<% dim lucur(3) as string
 lucur(2) = Today.Year.ToString
     lucur(1) = Today.Month.ToString
     lucur(0) = Today.Day.ToString
 dim sdate as string=lucur(1) & "/" & lucur(0) & "/" & lucur(2)
 response.write(sdate) %>"/>
<tr><td colspan='4'><input type='button' name='btnSave' id='btnSave' value='Save' />
<input type='reset' onclick="javascript:$('#btnSave').attr('title','Save');$('#btnSave').attr('value','Save');" /></td></tr></table></form>
</div>
    <sup style="color:Red;">*</sup>Required Fields
    </div>
 </div> 
 
    
 <div id="listx" style=" width:100%; padding:0px 7px 0px 0px;">
    <%  Dim db As New dbclass
        Dim dt As DataTableReader
        Dim sec As New k_security
        Dim outlast As String = ""
        If keyp = "update" Then
            'response.Write(request.QueryString("id"))
      dt = db.dtmake("new" & Today.ToLocalTime, "select * from emp_ot where id=" & Request.QueryString("id"), session("con"))
      If dt.HasRows = True Then
                dt.Read()
   
          Response.Write("<script type='text/javascript'>")
                Response.Write("$('#vname').val('" & fm.getfullname(fm.getinfo2("select emp_id from emprec where id=" & dt.Item("emptid"), Session("con")), Session("con")) & "');")
                Response.Write("$('#id').val('" & Request.QueryString("id") & "');")
                For i As Integer = 0 To arrx.Length - 1
                    Response.Write("$('#" & arrx(i) & "').attr('disabled',true);")
                    ' $('#editBtn').removeAttr('disabled')
                Next
                For k As Integer = 0 To dt.FieldCount - 3
                    'Response.Write("//" & dt.GetDataTypeName(k).ToLower)
                    ' Response.Write("alert('" & dt.GetName(k) & "');")
                    If LCase(dt.GetDataTypeName(k)) = "string" And dt.IsDBNull(k) = False Then
                        If dt.GetName(k) = "factored" Then
                            Response.Write("$('#" & dt.Item("factored") & "').removeAttr('disabled');")
                            Response.Write("$('#" & dt.Item("factored") & "').val('" & dt.Item("time_diff") & "');")
                            
                       
                        Else
                            
                              %>
                    $('#<%  Response.Write(dt.GetName(k) & "').val('" & dt.Item(k).trim & "');")
                            
                            
                            %>
                  
                    <%
                        End If
                        
                        
                      
                    
                    ElseIf LCase(dt.GetDataTypeName(k)) = "datetime" And dt.IsDBNull(k) = False Then
                        Dim sdatex As Date = dt.Item(k)
                        Dim d As String = sdatex.ToShortDateString
                        Dim da As String = sdatex.Day
                        Dim mm As String = sdatex.Month
                        Dim yy As String = sdatex.Year
                        d = mm & "/" & da & "/" & yy
                    Response.Write("$('#" & dt.GetName(k) & "').val('" & d & "');")
                    If dt.GetName(k) = "datework" Then
                        outlast = ("showHideSubMenux('" & sec.Str2ToHex(dt.Item("datework")) & "','tbl" & sec.Str2ToHex(dt.Item("datework")) & "');")
                    End If
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
            Dim loc As String
            loc = Request.ServerVariables("url").Substring(1, Request.ServerVariables("url").Length - 1)
        Dim sqlx As String = "select id,ot_date,datepaid,factored,time_diff,rate,amt,emptid from emp_ot order by ot_date desc"
        row = edit_del_list_wname_ot("emp_ot", sqlx, "Emp. Name,Overtime Date,Overtime Paid date,Factored-hour,HRs,Rate,Total Amount", Session("con"), loc)
        Response.Write(row)
        
    %>
 </div>
<div id="newpage"></div>
<script type="text/javascript" language="javascript">
hform=findh(document.getElementById("middle_bar"));
    $('.titlet').text("Overtime Reg.");
    //showobjar("formx","titlet",22,2);
   
  </script>
  
    <script type="text/javascript">
      function del(val,ans,hd)
       {
        
            if(ans=="yes")
            {
           // alert(val + ans);
         $('#frmx').attr("target","_self");
        $('#frmx').attr("action","<% response.write(loc) %>?task=delete&id="+val+"&tbl=emp_ot");
        $('#frmx').submit();
            }
            else
            {
                ha(hd);
            }
       }
   </script>
    <form id="frmx" action="" method="post">
    </form>
   
   
   <%  ' Response.Write(keyp)
       If keyp = "delete" Then
           Dim fs As New file_list
           Dim con As String
           Dim str As String
           con = "<span style='color:red;'> This row of data will not be come again.<br />Are you sure you want delete it?<br /><hr>" & _
           "<img src='images/gif/btn_delete.gif' style='cursor:pointer;' onclick=" & Chr(34) & "javascript:del('" & idx & "','yes','del123');" & Chr(34) & "></span>"
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
	
		$( "#dialog-modal" ).dialog({
		resizable: true,
			modal: true
		});
           </script>
           <%
           
       End If
           If flg = 1 Then%>
    <script type="text/javascript">
        //$(document).delay(80000);
        $('#frmx').attr("target","_parent");
       
        //$('#frmx').attr("action","<% response.write(rd) %>");
       // $('#frmx').submit();
   
    </script>
    
    <% 
    End If
    If outlast <> "" Then
    %>
    <script type="text/javascript">
        <% response.write(outlast) %>
    </script>
    <%  
    End If
       If Request.QueryString("takename") <> "" Then
        Response.Write("<script>$('#vname').val('" & Request.QueryString("vname") & "');</script>")

       End If
       
       %>
   
   
</body>
</html>

<script src="scripts/kirsoft.required.js" type="text/javascript"></script>

