<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="home.aspx.vb" Inherits="home" %>
<%@ Import Namespace="kirsoft.hrm" %>
    <asp:Content ID="top_cont_cont" ContentPlaceHolderID="top_cont" runat="Server"></asp:Content>
        <asp:Content ID="menu_cont_cont" ContentPlaceHolderID="menu_cont" runat="Server">    	
        </asp:Content>
               <asp:Content ID="ifram_cont_cont" ContentPlaceHolderID="ifram_cont" runat="Server">
                 <iframe id="frm_tar" name="frm_tar"  style="display:block;margin-left:auto;margin-right:auto;width:100%;" 
    frameborder="0" scrolling="auto" height="100%" src="homepage.aspx"></iframe>
    <iframe id="chksess" name="chksess" style=" position:absolute; width:100px; height:100px; top:0px; left:0px; visibility:absolute;" src="checksession.aspx"></iframe>
                    </asp:Content>
                  
                
              