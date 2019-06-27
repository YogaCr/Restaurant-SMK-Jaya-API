Imports System.Data
Imports System.Data.Entity
Imports System.Data.Entity.Infrastructure
Imports System.Linq
Imports System.Net
Imports System.Net.Http
Imports System.Web.Http
Imports System.Web.Http.Description
Imports Restaurant_SMK_Web_API

Namespace Controllers
    Public Class LoginController
        Inherits System.Web.Http.ApiController

        Private db As New RestaurantSMKEntities

        <HttpPost> <Route("api/login")>
        Function Login()
            Dim email = Request.Headers.GetValues("email").FirstOrDefault
            Dim password = Request.Headers.GetValues("password").FirstOrDefault
            Dim res = From e In db.MsEmployees
                      Where e.Email = email And e.Position = "Waiter"
                      Select e
            If res.Count > 0 Then
                If res.SingleOrDefault.Password = password Then
                    Return Request.CreateResponse(HttpStatusCode.OK, New With {.Message = "Login sukses"})
                End If
            End If
            Return Request.CreateResponse(HttpStatusCode.Forbidden, New With {
            .Message = "Email atau password salah atau anda tidak diizinkan mengakses aplikasi ini"})
        End Function
    End Class
End Namespace