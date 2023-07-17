<%@ Page Language="VB" AutoEventWireup="false" CodeFile="emplist.aspx.vb" Inherits="emplist" %>
<%@ Import Namespace="system.IO" %>
<%@ Import Namespace="system.data" %>
<%@ Import Namespace="system.data.sqlclient" %>
<%@ Import Namespace="Kirsoft.hrm" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>

	
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
	<script type="text/javascript" src="scripts/kirsoft.iframe.js"></script>
	
	<title></title>
    <script type="text/javascript">
   var where=new Array(6); 
   function mksql()
   {
           var strwhere="";
                         where[0]="";
            if($("#empsearch_name").val()!="")
            {   
                where[0]="first_name like '" + $("#empsearch_name").val() + "%'";
            }
            where[1]="";
               if($("#empid").val()!="")
            {   
                where[1]="emp_id like '" + $("#empid").val() + "%'";
            } 
             where[2]="";
            if($("#emptype").val()!="")
            {   
                where[2]="type_recuritment like '" + $("#emptype").val() + "%'";
            }  
            where[3]="";
            if($("#emp_dep").val()!="")
            {   
                where[3]="department like '" + $("#emp_dep").val() + "%'";
            }
            where[4]="";
            if($("#empstate").val()!="")
            {   
                if($("#empstate").val()=="n")
                where[4]="active = '" + $("#empstate").val() + "' and first_name in(select first_name from vwemp_emp_dep_qual where end_date is not null)";
                else
                where[4]="active = '" + $("#empstate").val() + "'";

            }
            else if ($("#empstate").val() == "") {
               
                    where[4] = "";
              

            }
            where[5]="";
             if($("#empqul").val()!="")
            {   
                where[5]="qualification = '" + $("#empqul").val() + "'";
            }
           var k=0;
           var kw=0;
            for(i=0;i<6;i++)
            {
                if(where[i]!="")
                {  if(k==0)
                    {
                        k=1;
                        strwhere=strwhere+"where ";
                    }
                   
                        
                    
                    strwhere  =  strwhere + where[i]; 
                   strwhere= strwhere + " and "  ; 
                }
                
             }
          var and=strwhere.substr(strwhere.length-4,strwhere.length);
           if(and=="and ")
           {
           
            if (strwhere.search("active")<=0)
                           strwhere=strwhere.substr(0,strwhere.length-4);
                           else
                strwhere=strwhere.substr(0,strwhere.length-4);
          }
      
    var sql="select * from vwemp_emp_dep_qual " + strwhere;
 // $("#textone").text(sql);
    $('#search_form').attr('target', 'listview');
    $('#listview').css({'height':'350px'});
        $('#search_form').attr('action','listemp.aspx?sql=' + sql); 
       $('#search_form').submit();
   }
        function onfocus_click(id)
        {
            //$('#' + id).append(new Option("kir","kir"));
        }
    </script>
</head>
<body style="">
<div></div>

<div id="textone">
    </div>
<%  If Session("emp_id") <> "" Then
        Session("emp_id") = Nothing
        Session("fullempname") = Nothing
        Session("emp_path") = Nothing
    End If
    Session("emptid") = ""
    %>
<div style=" ">
  <div>
    <div>
      <h2>Employee Information</h2>
    </div>
    <div>
      <form id="search_form" method="post" action="">
        <div id="formcontent"><br />
          <label for="empsearch_employee_name">Employee Name</label>
          <input name="empsearch_name" value="" id="empsearch_name"/>
          <label for="empid">Id</label>
          <input type="text" name="empid" id="empid" />
          <label for="emptype">Employment Type</label>
          <select name="emptype" id="emptype">
             
            <option value="">All</option>
          
<%  Dim fm As New formMaker
    Response.Write(fm.getoption("tbl_emp_type where status='y'", "emptype", "emptype", session("con")))%>

             
          </select>
           <label for="empqul">Employee Qualification</label>
          <select name="empqul" id="empqul">
             
            <option value="">All</option>
          
<%  
    Response.Write(fm.getoption("tblqualification where stat='y'", "qualification", "qualification", session("con")))%>

             
          </select>
          <label for="empstate">Include</label>
          <select name="empstate" id="empstate">
             
            <option value="y">Active Employees</option>
             
            <option value="" selected="selected">Active and Terminated Employees</option>
             
            <option value="n">Terminated Employees </option>
             
          </select>
          <br />
          <label for="emp_dep">Department:</label>
          <select name="emp_dep" id="emp_dep">
             <option value="">All</option>
               <%  
                   Response.Write(fm.getoption("tbldepartment", "dep_id", "dep_name", session("con")))%>
 </select> <script type="text/javascript">
          
          </script>
          <div>
            <div>
              <!--input type="button" id="searchBtn" onmouseover="this.className='plainbtn plainbtnhov'" onmouseout="this.className='plainbtn'" value="Search" name="_search" />
              <input type="button" onmouseover="this.className='plainbtn plainbtnhov'" id="resetBtn" onmouseout="this.className='plainbtn'" value="Reset" name="_reset" /-->
            </div>
            <br />
          </div>
          <br />
        </div>
      </form>
    </div>
  </div>
  
</div>

<% 
    If Session("username") = "" Then
        Response.Redirect("logout.aspx")
    End If
  %> 
    <div id="divlist" style="overflow:auto; height: auto; vertical-align:top">
    <iframe id='listview' scrolling="auto" name='listview' style=" width:100%; border:none; overflow:hidden; height: 350px;" frameborder="0" scrolling="yes" src="listemp.aspx" ></iframe> </div>
   
    
</body>
</html>
<script language="javascript" type="text/javascript">
 $(document).ready(function(){
  $('#search_form').keyup(function(){
  mksql();
       // var sql="select * from emp_static_info where first_name like '" + $('#empsearch_name').val() + "%'";
        //$('#search_form').attr('target','listview');
        //$('#search_form').attr('action','listemp.aspx?sql=' + sql); 
        //$('#search_form').submit();
        
    });    
   $('#search_form').change(function(){
  mksql();
       // var sql="select * from emp_static_info where first_name like '" + $('#empsearch_name').val() + "%'";
        //$('#search_form').attr('target','listview');
        //$('#search_form').attr('action','listemp.aspx?sql=' + sql); 
        //$('#search_form').submit();
        
    });    
       $(document).height('310');
      
    });
  //$("#divlist").load("listemp.aspx");


</script>

