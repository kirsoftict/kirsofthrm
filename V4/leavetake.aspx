<%@ Page Language="VB" AutoEventWireup="false" CodeFile="leavetake.aspx.vb" Inherits="leavetake" %>
<%@ Import Namespace="kirsoft.hrm" %>
<%@ Import Namespace="system.data" %>
<%@ Import namespace="System.Data.sqlclient"%>
<%  If Session("username") = "" Then
       %>
       <script type="text/javascript">
           //document.location.href="admin_home.php"
          // window.location = "logout.aspx";
</script>
       <%
       End If
       
       If Session("emp_id") = "" Then
       %>

<script type="text/javascript">
    //document.location.href="admin_home.php"
    window.location = "empcontener.aspx";
</script>

<%
ElseIf Session("emptid") = 0 Then

     %>

<script type="text/javascript">
    alert('<% response.write(session("emptid")) %>');
</script>

<%    
End If

    dim emptid as string=session("emptid")
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
            sql = dbx.makeupdate_statement(tbl, Request.QueryString, Session("con"), key, keyval)
            Response.Write(sql)
            flg = dbx.save(sql, session("con"),session("path"))
            Dim val As Double
            val = Request.QueryString("no_days")
            Dim rs As DataTableReader
            rs = dbx.dtmake("empleave", "select * from empleavapp where req_id='" & keyval & "' order by id desc", Session("con"))
            If rs.HasRows = True Then
                While rs.Read
                    If CDbl(rs.Item("used")) - CDbl(val) >= 0 Then
                        msg = "update empleavapp set used='" & val.ToString & "' where id=" & rs.Item("id")
                        dbx.save(msg, Session("con"), Session("path"))
                    Else
                        msg = "update empleavapp set used='" & rs.Item("used") & "' where id=" & rs.Item("id")
                        val = val - CDbl(rs.Item("used"))
                        dbx.save(msg, Session("con"), Session("path"))
                    End If
                End While
                
            End If
            rs.Close
                    ' Response.Write(sql)
                    If flg = 1 Then
                msg = "Data Updated"
                    End If
                ElseIf Request.QueryString("task") = "delete" Then
              
                    sql = "delete from " & Request.QueryString("tbl") & " where id=" & Request.QueryString("id") '
                    flg = dbx.save(sql, session("con"),session("path"))
                   
                    ' Response.Write(sql)
            If flg = 1 Then
                sql = "delete from empleavapp where req_id=" & Request.QueryString("id") 'deleted from budget
                flg = dbx.save(sql, session("con"),session("path"))
                msg = "Data deleted"
            End If
                ElseIf Request.QueryString("task") = "save" Then
                If Request.QueryString("date_taken_from") <> "" Then
                Dim dtstr() As String = Request.QueryString("date_taken_from").Split("/")
                flg = 0
                Dim dtc As New datetimecal
                Dim ksys As New kirsoftsystem
                    
                Dim ddd As Date
                ddd = "#" & Request.QueryString("date_taken_from") & "#" '"#" & dtstr(1) & "/" & dtstr(0) & "/" & dtstr(2) & "#"
                    
                '  ddd = ksys.StringToDate((Request.QueryString("date_taken_from")), "dd/mm/yyyy")
                Dim obj, obj2 As Object
                obj = dtc.isWeekEnd(ddd, Session("emp_id"), session("con"))
                obj2 = dtc.isPublic(ddd, session("con"))
                msg = obj.ToString
                    
                If obj.ToString = "True" Then
                    flg = 2
                    msg = "This day is your weekend, please select another day"
                End If
                If obj2 = True Then
                    flg = 2
                    msg = "This day is Public Holiday"
                End If
                If obj2 = True And obj.ToString = "True" Then
                    flg = 2
                    msg = "This day is Public and week holiday"
                End If
                If flg <> 2 Then
                    sql = dbx.makest(tbl, Request.QueryString, session("con"), key)
                    ' Response.Write(sql)
                    flg = dbx.save(sql, session("con"),session("path"))
                    If flg = 1 Then
                        msg = "Data Saved"
                    Else
                        msg = "Sorry Data is not Save"
                    End If
                End If
                    
            Else
                    
                msg = "it is else"
            End If
               
                ' Response.Write(sql)
               
            ElseIf Request.QueryString("task") = "deletefile" Then
                Response.Write("adfadf")
                
                If Request.QueryString("id") <> "" Then
                    
                    paths = Request.QueryString("id")
                    paths = paths.Replace("~", "\")
                    msg = fl.deletefile(paths)
                End If
                flg = 1
            End If
               
            If flg <> 1 Then
                'Response.Write("<span style='font-size:15pt; color:Red;'>Sorry Data doesnt change </span>")
            End If
          
   
        End If
    End If
   
    paths = ""
    loc = ""
    loc = Request.ServerVariables("url").Substring(1, Request.ServerVariables("url").Length - 1)
   %>   
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>
</title>

<link rel="stylesheet" href="jqq/themes/base/jquery.ui.all.css">
	<script src="jqq/jquery-1.9.1.js"></script>
	<script src="jqq/ui/jquery.ui.core.js"></script>
	<script src="jqq/ui/jquery.ui.widget.js"></script>
	<script src="jqq/ui/jquery.ui.position.js"></script>
	<script src="jqq/ui/jquery.ui.menu.js"></script>
	<script src="jqq/ui/jquery.ui.autocomplete.js"></script>
<script type="text/javascript" src="scripts/form.js"></script>
    <script src="jqq/ui/jquery.ui.button.js"></script>

    <script src="jqq/ui/jquery.ui.dialog.js"></script>
		<script src="jqq/ui/jquery.ui.datepicker.js"></script>

<script src="scripts/script.js" type="text/javascript"></script>

	
<link rel="stylesheet" type="text/css" media="screen" href="css/kir.login.css" /> 
<script src="scripts/kirsoft.java.js" type="text/javascript"></script>
<script src="scripts/kirsoft.required.js" type="text/javascript"></script>
<script type="text/javascript">
function showHideSubMenu(link) {

        var uldisplay;
        var newClass;

        if (link.className == 'expanded') {

            // Need to hide
            uldisplay = 'none';
            newClass = 'collapsed';

        } else {

            // Need to show
            uldisplay = 'block';
            newClass = 'expanded';
        }

        var parent = link.parentNode;
        uls = parent.getElementsByTagName('ul');
        for(var i=0; i<uls.length; i++) {
            ul = uls[i].style.display = uldisplay;
        }

        link.className = newClass;
    }
var prv;
  prv="";
var id;
var focused="";
var requf=["emp_id","leave_type","reason","date_taken_from","no_days","requested_date","x"];
var fieldlist=["emp_id","leave_type","reason","date_taken_from","no_days","byhalfday","date_return","requested_date","approved_dep","approve_dep_date","approved_by","approved_date","canceled","remark","x"];
function validation1(){
if ($('#emp_id').val() == '') {showMessage('emp_id cannot be empty','emp_id');$('#emp_id').focus();return false;}
if ($('#leave_type').val() == '') {showMessage('leave_type cannot be empty','leave_type');$('#leave_type').focus();return false;}
if ($('#reason').val() == '') {showMessage('reason cannot be empty','reason');$('#reason').focus();return false;}
if ($('#date_taken_from').val() == '') {showMessage('date_taken_from cannot be empty','date_taken_from');$('#date_taken_from').focus();return false;}
if ($('#no_days').val() == '') {showMessage('no_days cannot be empty','no_days');$('#no_days').focus();return false;}
if ($('#no_days').val()!=''){
var nval;
 var ans;
 var str;
nval=$('#no_days').val();
if((parseFloat(nval)-parseInt(nval))>0.5)
{ showMessage('in no_days the decimal cannot be less than or greater than 0.5','no_days');$('#no_days').focus();return false;}
else if((parseFloat(nval)-parseInt(nval))<0.5 && (parseFloat(nval)-parseInt(nval))>0 )
{showMessage('in no_days the decimal cannot be less than or greater than 0.5','no_days');$('#no_days').focus();return false;}
else if(focused=="") {
ans=checkblur();
if(ans!=true){ 
 $("#" + ans).focus();
}else{
   str=$("#frmemp_leave_take").formSerialize();
   //alert(str);
   $("#frmemp_leave_take").attr("action","?tbl=emp_leave_take&task=<% response.write(keyp) %>&lrd=empcontener&key=id&keyval=<% response.write(idx) %>&rd=empcontener.aspx&" + str);
    $("#frmemp_leave_take").submit();
  return true;}
  }
}
else if(focused=="") { 
ans=checkblur();
if(ans!=true){ 
 $("#" + ans).focus();
}else{
   str=$("#frmemp_leave_take").formSerialize();
   //alert(str);
   $("#frmemp_leave_take").attr("action","?tbl=emp_leave_take&task=<% response.write(keyp) %>&lrd=empcontener&key=id&keyval=<% response.write(idx) %>&rd=empcontener.aspx&" + str);
    $("#frmemp_leave_take").submit();
  return true;}
  }
} 
function goupload(whr,fn)
{
    var ftype=".doc,.docx,.pdf,.jpg";
    var size=1000000;
    var loct="<%response.write(session("emp_id")) %>/leave/" +fn+"/";
  var hw= window.screen.availHeight;
  var ww=window.screen.availWidth;
   // alert(whr + fn.toString());
     $('#frmx').attr("target","uploadf");
       
       $('#frmx').attr("action","allupload.aspx?upload=phase1&ftype="+ftype+"&size="+size+"&loct="+loct);
       $('#frmx').submit();
       $('#upload').css({top:'0px',left:'0px'});
       showobj("upload");
 // $( '#upload').dialog('destroy');
//$( '#upload' ).dialog({resizable: true,modal: true});
  //   $('#upload').css({'visibility':'visible','display':'inline'});
}
function del(val,ans)
       { 
        if( ans=="1st")
          { // alert(val);
                $('#frmx').attr("target","_self");
                $('#frmx').attr("action","<% response.write(loc) %>?task=deletefile&id="+val+"&tbl=file");
                $('#frmx').submit();
            }
        else
        {
       // str=$("#frmemp_static_info").formSerialize();
        //alert("deleted");
         //  $("#messagebox").load("deletefile.aspx?fname=" + val + "&tasks=delete");

              $('#frmx').attr("target","_self");
             $('#frmx').attr("action","<% response.write(loc) %>?task=deletecon&id="+val+"&delete=true");
             $('#frmx').submit();
        }
            
       }
        function approvednow(val)
        {
       
            //var str=$("#appvd").formSerialize();
           // alert(str);
            $("#appvd").attr("action", val + "&");
           // alert(val);
            $("#appvd").submit();
            
           // return true;
        }
        
       function approved(val,ans)
       { 
         //$('#approvedx').load("leavecalc.aspx?id="+val+"&task="+ans+"&tbl=emp_leave_take");
       
             $('#frmx').attr("target","appf");
       
       $('#frmx').attr("action","leavecalc.aspx?id=" + val + "&task=" +ans);
       $('#frmx').submit();
       $('#approved').css({top:'0px',left:'0px',width:'1100px',height:'1000px',background:'#ffffff'});
       showobj("approved");
       }
        </script>
<script type="text/javascript" language="javascript">
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
<br />
<%  ' Dim fl As New file_list
    Response.Write(fl.msgboxt("upload", "Upload", "<iframe name='uploadf' id='uploadf' width='500' height='250' frameborder='0' src='' scrolling='no'></iframe>"))
    Response.Write(fm.popupwindow("approved", "Approved", "<iframe name='appf' id='appf' width='900' height='1000' frameborder='0' src='' scrolling='yes'></iframe>"))%>
<div>
      <%
          Dim floc As String
          Dim db As New dbclass
          Dim dt As DataTableReader
          '  Dim loc As String
          Dim noav As Double = 0
          Dim nobgt As Double = 0
          Dim nobal As Double = 0
          Dim nouse As Double = 0
          Dim strview As String = ""
          Dim nod As Integer = 0
          Dim dt2 As DataTableReader
          Dim sumavx As Double
          Dim cout, give, v As Double
          Dim outp As String
          Dim sumav As Double
          Dim dx As Date
          Dim r() As Object
          Dim byhalf As String = "n"
          sumav = 0
      
          Dim rid As Integer
          Dim bgt, nd As String
          Dim req As String
          Dim redirect As String = ""
          Dim appdate As Date
          Dim finof As String = ""
          Dim res As String = ""
          Dim sql2(1) As String
          give = 0
          bgt = ""
          nd = ""
          req = ""
          Dim lucur(3) As String
          Dim sdate As String = ""
          Dim i As Integer = 0
          Dim rqid As Integer
          If Request.QueryString("approved") = "on" Then
              If Request.QueryString("std") = "on" Then
                  Response.Write(newapproved(1))
              Else
                  Response.Write(newapproved(2))
              End If
             
          ElseIf Request.QueryString("other") = "on" Then
                 
              sql = "update emp_leave_take set date_return='" & Request.QueryString("datereturn") & "',approved_by='" & Session("emp_iid") & "',approved_date='" & Request.QueryString("appdate") & "' where id=" & Request.QueryString("id")
              db.save(sql, session("con"),session("path"))
              ' Response.Write(sql)
          End If
          Dim color As String
          Try
              Response.Write(fm.getleaveinfo(emptid, Session("con")))
          Catch ex As Exception
              Response.Write(ex.ToString & "<br>")
              Response.Write(getleaveinfo(emptid, Session("con")))
          End Try
          
          ' Response.Write(fm.getleaveinfo2(emptid, Session("con")))
          Dim rtnx(), rtny() As String
          Dim rtnbal() As Double
          rtnx = newcalc()
          rtny = newcalc2()%>
  
      
                        <%
                            rtnbal = getleaveinfox(Session("emptid"), Session("con"))
       sql = "select * from emp_leave_take where approved_date is null and emptid=" & Session("emptid")
          
             Try
                 dt = db.dtmake("thscal", sql, session("con"))
             Catch ex As Exception
                %>
       <script type="text/javascript">
           //document.location.href="admin_home.php"
          // window.location = "logout.aspx";
</script>
       <%
           'Response.Redirect("logout.aspx")
           Response.Write(ex.ToString & sql)
          End Try
              
       Dim lvt As String
       Dim loan As Double
       Dim rslt As String
       sql = "select sum(isnull(loan,0)) as tloan from emp_leave_take where emptid=" & Session("emptid")
       Try
           rslt = fm.getinfo2(sql, Session("con"))
           If IsNumeric(rslt) Then
               loan = CDbl(rslt)
           Else
               loan = 0
           End If
           
       Catch ex As Exception
           loan = 0
           Response.Write(sql)
       End Try
       
       
       sql = "select * from show_leave_bal where emp_id='" & Session("emp_id") & "' and emptid=" & Session("emptid") 
      
       dt2 = db.dtmake("thscals", sql, Session("con"))
          If dt.HasRows = True Then
              
              dt.Read()
              req = dt.Item("no_days")
           rqid = dt.Item("id")
           ' Response.Write(fm.saveleave(dt.Item("emptid"), rqid, Session("con")))
           give = CDbl(req)
           lvt = LCase(dt.Item("leave_type"))
              If LCase(dt.Item("leave_type")) = "annual leave" Then
                  byhalf = dt.Item("byhalfday")
                  dx = dt.Item("date_taken_from")
                  res = dt.Item("reason")
              
                  If dt2.HasRows = True Then
                      strview = "<table cellspacing='0' cellpadding='0' width='900px'>" & _
                  "<tr style='background:#7595f7; padding:0px 10px 0px 0px; font-size:9pt;font-weight:bold;'>"
                      For j As Integer = 1 To dt2.FieldCount - 1
                          strview &= "<td  style='padding-right:10px;'>" & dt2.GetName(j) & "</td>"
                      Next
                      strview &= "</tr>"
                   ' Dim color As String = "E3EAEB"
                      ' Dim remx As Double
                      nod = dt.Item("no_days")
                      cout = 0
                      sumavx = 0
                      While dt2.Read
                                 
                          If color <> "#E3EAEB" Then
                              color = "#E3EAEB"
                          Else
                              color = "#fefefe"
                          End If
                          If fm.isexp(Today.ToShortDateString, dt2.Item("Year End"), 2, "y") Then
                              color = "red"
                       Else
                           ' Response.write(dt2.Item("Year End"))
                           v = fm.showavdate(dt2.Item("Year Start"), dt2.Item("Year End"), dx, dt2.Item("Balance"))
                           noav = noav + v
                           '  Response.Write(v & "<br>")
                           nobgt = nobgt + dt2.Item("Budget")
                           'Response.Write(nobgt & "<br>")
                           nobal = nobal + dt2.Item("Balance")
                           nouse = nouse + dt2.Item("Used")
                           If give > 0 Then
                               'dx = dt.Item("date_taken_from")
                               If give <= v Then
                                   bgt &= dt2.Item("id") & ","
                                   nd &= give.ToString & ","
                                   give = give - v
                               Else
                                   
                                   bgt &= dt2.Item("id") & ","
                                   nd &= v.ToString & ","
                                   give = give - v
                               End If
                           End If
                          End If
                          strview &= "<tr style='background:" & color & ";padding:0px -1px 0px 0px;'>"
                          For i = 1 To dt2.FieldCount - 1
                              If dt2.IsDBNull(i) = False Then
                                  strview &= "<td  style='padding-right:10px;'>" & dt2.Item(i) & "</td>"
                              Else
                                  strview &= "<td>&nbsp;</td>"
                              End If
                          Next
                          strview &= "</tr>"
                          cout += 1
                      End While
                      strview &= "<tr style='font-weight:bold;' bgcolor='gray'><td colspan='4' align='middle'>Summary</td><td>" & nobgt.ToString & "</td><td>" & nouse.ToString & "</td><td>" & nobal & "</td></tr>"
                  End If
                  ' Dim r() As Object
               r = fm.calcwhen3(dt.Item("date_taken_from"), dt.Item("no_days"), Session("emp_id"), Session("con"), byhalf)
                  strview &= "</table>"
           
               outp = "<div id='uproved'><table>" & _
                  "<tr><td colspan='6'><b>Leave starts on:</b><u>" & r(0) & "</u> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<b>Date Return:</b><u>" & r(5) & "</u>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <b>Leave requested:</b><u>" & req.ToString & "</u>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <b>By Half Days:</b><u>"
               If byhalf = "y" Then
                   outp &= " Yes</u></td></tr></table> "
               Else
                   outp &= " No Full days</u></td></tr></table> "
               End If
               outp &= "<table>" & r(6) & "</table>"
               If rtnbal(2) < 0 Then
                   outp &= "<span style='color:red;'><br>Note: Please notice that there is Deficient Days by " & Math.Round(rtnbal(2) - req, 2) & " included this leave request! <br> it will be approved using your authority</b></span>"
           
               ElseIf noav < req Then
                   outp &= "<span style='color:red;'><br>Note: Please notice that there are short days from request by " & Math.Round(req - noav) & " <br>those days are not taken by the system</b></span>"
                   ' Response.Write(nobgt)
               End If
               outp &= "</div>"
               outp &= "<div id=" & Chr(34) & "print" & Chr(34) & " style=" & Chr(34) & " float:right; width:59px; height:33px; color:Gray;cursor:pointer" & Chr(34) & " onclick=" & Chr(34) & "javascirpt:print('uproved','Report_print','" & Session("company_name") & "<BR> Leave Summery','" & Today.ToLongDateString & "');" & Chr(34) & ">print</div>"
                   
               ' Response.Write(strview)
               Response.Write(outp)
               
                   %>
                   <form id="appvd" name="appvd" method="post" action=""> 
                   Date:<input type="text" id="appdate" name="appdate" value="<% 
 lucur(2) = Today.Year.ToString
     lucur(1) = Today.Month.ToString
     lucur(0) = Today.Day.ToString
  sdate =lucur(1) & "/" & lucur(0) & "/" & lucur(2)
 response.write(sdate) %>"/>
                   <script language='javascript' type='text/javascript'>
                       $(function () {
                           $("#appdate").datepicker({ changeMonth: true, changeYear: true });
                           $("#appdate").datepicker("option", "dateFormat", "mm/dd/yy");
                       });
                   </script>
                   <input type="hidden" id='hiddenpass' name='hiddenpass' value="<% response.write(rtnx(0)) %>" />
                   <a href="javascript:approvednow('?approved=on&id=<% response.write(rqid) %>&datereturn=<%response.write(r(5).tostring) %>&std=on&rid=<%response.write(rid) %>&bgt=<%response.write(bgt) %>&nd=<%response.write(nd) %>&remain=<%response.write(rtnx(1)) %>');">approved</a> 
                 &nbsp; | &nbsp; <a href="javascript:approvednow('?approved=on&id=<% response.write(rqid) %>&datereturn=<%response.write(r(5).tostring) %>&rid=<%response.write(rid) %>&bgt=<%response.write(bgt) %>&nd=<%response.write(nd) %>&remain=<%response.write(rtny(1)) %>');">approved<label style="color:Gray;font-size:8pt;">(deduct from expired portion)</label></a>
                   </form>
                   <%  
                       Else
                           If dt.HasRows Then
                           r = fm.calcwhen3(dt.Item("date_taken_from"), dt.Item("no_days"), Session("emp_id"), Session("con"), byhalf)
                           outp = "<div id='uproved'><table><tr><td colspan='6'>" & _
                    "Please the down information comes only once if you want print it and have it for Evidence" & _
                    "</td></tr>" & _
                   "<tr><td colspan='6'><b>Leave starts on:</b><u>" & r(0) & "</u> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<b>Date Return:</b><u>" & (CDate(r(5)).AddDays((r(1) + r(2)) * -1)) & "</u>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <b>Leave Requested:</b><u>" & req.ToString & "</u></td><td><b>Leave type:</b><u>" & dt.Item("leave_type") & "</u></td></tr></table> </div>"
                         
                           outp &= "<div id=" & Chr(34) & "print" & Chr(34) & " style=" & Chr(34) & " float:right; width:59px; height:33px; color:Gray;cursor:pointer" & Chr(34) & " onclick=" & Chr(34) & "javascirpt:print('uproved','Report_print','" & Session("company_name") & "<BR> Leave Summery','" & Today.ToLongDateString & "');" & Chr(34) & ">print</div>"
                           ' Response.Write(r(3))
                           Response.Write(outp)
                           ' Response.Write("see what wee see")
                        %>
                        <form id="appvd" name="appvd" method="post" action="">
                         Date:<input type="text" id="appdate" name="appdate" value="<% 
 lucur(2) = Today.Year.ToString
     lucur(1) = Today.Month.ToString
     lucur(0) = Today.Day.ToString
  sdate=lucur(1) & "/" & lucur(0) & "/" & lucur(2)
 response.write(sdate) %>"/>
                   <script language='javascript' type='text/javascript'>                       $(function () {
                           $("#appdate").datepicker({ changeMonth: true, changeYear: true });
                           $("#appdate").datepicker("option", "dateFormat", "mm/dd/yy");
                       });
                   </script>
          <a href="javascript:approvednow('?other=on&id=<% response.write(rqid) %>&datereturn=<%response.write((CDate(r(5)).AddDays((r(1) + r(2)) * -1)).tostring) %>&rid=<%response.write(rid) %>&bgt=<%response.write(bgt) %>&nd=<%response.write(req) %>');">approved</a>

                   </form>
             <% 
             End If
             End If
         End If%>
   
   </div>
<div id="lcalc"></div>
<div id="iframeh" style="visibility:hidden; width:300px; height:100px; overflow:hidden; display:none;" >
</div>
<div>
 </div>
   <% 'ponse.Write(Session("fullempname"))




'Response.Write("<br />" & Request.Form("do"))

 %>
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
   <form method='post' id='frmemp_leave_take' name='frmemp_leave_take' action=""> 
<table><tr><td><input type='hidden' id='emp_id' name='emp_id' value='<% response.write(session("emp_id")) %>' />
<input type='hidden' id='emptid' name='emptid' value='<% response.write(session("emptid")) %>' />
Leave Type<sup style='color:red;'>*</sup></td><td>:</td><td>
<select id="leave_type" name="leave_type">
<option value="">leave type</option>
<%  
    Response.Write(fm.getoption("tbl_leave_type", "leave_type", "leave_type", session("con")))
    
    %>
</select>
</td>
</tr><tr><td>Reason<sup style='color:red;'>*</sup></td><td>:</td><td><input type='text' id='reason' name='reason' value='Personal' /><br />
<label class='lblsmall'></label></td>
<td>Date From<sup style='color:red;'>*</sup></td><td>:</td><td><input type='text' id='date_taken_from' name='date_taken_from' value='' />
<br /><label class='lblsmall'></label></td>
<script language='javascript' type='text/javascript'>
    $(function () {
        $("#date_taken_from").datepicker({ changeMonth: true, changeYear: true, minDate: "-10Y", maxDate: "+1Y" });
        $("#date_taken_from").datepicker("option", "dateFormat", "mm/dd/yy");
    });
</script></tr>
<tr><td>Half day<sup style='color:red;'>*</sup></td><td>:</td>
<td>
<select id='byhalfday' name='byhalfday'>
<option value='n' selected="selected">No</option>
<option value='y'>yes</option>
</select>
<label class='lblsmall'></label></td></tr>
<tr><td>No Days<sup style='color:red;'>*</sup></td><td>:</td>
<td><input type='text' id='no_days' name='no_days' value='' /><br /><label class='lblsmall'></label></td>
<td>Date return<sup style='color:red;'>*</sup></td><td>:</td><td>
<input type='text' id='date_return' name='date_return' value='' style="visibility:hidden;" />
<br /><label class='lblsmall'></label></td>
<script language='javascript' type='text/javascript'>
    $(function () {
        $("#date_return").datepicker({ changeMonth: true, changeYear: true, minDate: "-10Y", maxDate: "+1Y" });
        $("#date_return").datepicker("option", "dateFormat", "mm/dd/yy");
    });
</script>
</tr><tr><td colspan='4'><input type='hidden' id='requested_date' name='requested_date' value="<% 
 lucur(2) = Today.Year.ToString
     lucur(1) = Today.Month.ToString
     lucur(0) = Today.Day.ToString
 sdate =lucur(1) & "/" & lucur(0) & "/" & lucur(2)
 response.write(sdate) %>" />
 <input type='button' name='btnSave' id='btnSave' value='Save'  />
 </td></tr></table></form>
    </div>
    <sup style="color:Red;">*</sup>Required Fields
 </div>

   
 <div id="listx" style=" width:70%; padding:0px 7px 0px 0px; float:left">
    <%  
        '  Dim loc As String
        If keyp = "update" Then
          
      dt = db.dtmake("new" & Today.ToLocalTime, "select * from emp_leave_take where id=" & Request.QueryString("id"), session("con"))
      If dt.HasRows = True Then
          dt.Read()
                Response.Write("<script type='text/javascript'>")
                Response.Write("$('#date_return').css({visibility:'visible'});")

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
           
        Dim mk As New formMaker
        Dim row As String
        row = mk.getinfo2("select id from emp_leave_take where approved_date is not null and emptid=" & emptid & " order by id desc", session("con"))
        If row <> "" Then
            Response.Write("<a href='leaveform.aspx?id=" & row & "'>view</a>")
        End If
        row = ""
        floc = Server.MapPath("employee") & "/" & Session("emp_id") & "/leave"
        loc = Request.ServerVariables("url").Substring(1, Request.ServerVariables("url").Length - 1)
        Dim sqlx As String = "select id,leave_type,date_taken_from,no_days,byhalfday,date_return,Loan from emp_leave_take where emp_id='" & Session("emp_id") & "' order by id desc"
        row = mk.edit_del_list3("emp_leave_take", sqlx, "Leave type ,Start Date, Leave Taken, By Half days,Return Date,Loan ,Uploads,View,", Session("con"), loc, floc, True, True, True, True)
        Response.Write(row)
                    
        'files uploaded
    
    %>
 </div>
 <%
     Dim f As New file_list
     loc = Request.ServerVariables("url").Substring(1, Request.ServerVariables("url").Length - 1)
     Response.Write(filesview(Server.MapPath("employee"), Session("emp_id"), "leave", "employee"))%>
 <div></div>
 <div style=" float:left; width:30%;">
 <script type="text/javascript">
    function onchangebb()
    {
        var str=$("#upload").formSerialize();
        //alert("jqueryupdate.aspx?empid=<% response.write(session("emp_id")) %>&" + str + "&dest=leavetake&ftype=jpg,gif,png,doc,docx,pdf&task=upload");
       $("#newpage").load("jqueryupdate.aspx?empid=<% response.write(session("emp_id")) %>&" + str + "&dest=leavetake&ftype=jpg,gif,png,doc,docx,pdf&what_task=upload");
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
  
    
    <form id="frmx" action="" method="post">
    </form>
   <form id="frmup" action="" method="post" enctype="multipart/form-data">
    </form>
   
   <%  ' Response.Write(keyp)
       If keyp = "delete" Then
           Dim fs As New file_list
           Dim con As String
           Dim str As String
           con = "<span style='color:red;'> This row of data will not be come again.<br />Are you sure you want delete it?<br /><hr>" & _
           "<img src='images/gif/btn_delete.gif' style='cursor:pointer;' onclick=" & Chr(34) & "javascript:delx('" & idx & "','yes','del123');" & Chr(34) & "></span>"
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

           If flg = 1 Then%>
    <script type="text/javascript">
        //$(document).delay(80000);
        $('#frmx').attr("target","workarea");
        $('#frmx').attr("action","<% response.write(Request.ServerVariables("URL")) %>?msg=<% response.write(msg) %>" );
        $('#frmx').submit();
    </script>
   <%  ElseIf flg = 2 Then
       %><script type="text/javascript">
             showMessage('<%response.write(msg) %>', 'date_taken_from');
       </script>
       <%
   End If
       f = Nothing
       
       
       
       %>
   
    <script type="text/javascript">
        function delx(val, ans, hd) {

            if (ans == "yes") {
                // alert(val + ans);
                $('#frmx').attr("target", "_self");
                $('#frmx').attr("action", "<% response.write(loc) %>?task=delete&id=" + val + "&tbl=emp_leave_take");
                $('#frmx').submit();
            }
            else {
                ha(hd);
            }
        }
   </script>
<%
    dt = dbx.dtmake("stringcon", "select id from emp_leave_take where approved_date is null and emptid=" & Session("emptid") & " order by id desc", Session("con"))
    Dim savebtn As String = "disable"
    If dt.HasRows Then
        savebtn = "disabled"
    Else
        savebtn = "false"
    End If
    'Response.Write(savebtn)
    db = Nothing
    dt = Nothing
        
    
    
    loansettle()
    
    
    %>
</body>
</html>
<script type="text/javascript">
   $(document).ready(function() {
  <% if savebtn="disabled" then
   %>
   $("#btnSave").attr("disabled","<% response.write(savebtn) %>");
   <%else %>
    $("#btnSave").removeAttr("disabled");
   <%end if %>
   });
</script>