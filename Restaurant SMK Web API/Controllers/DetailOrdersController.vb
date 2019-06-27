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
    Public Class DetailOrdersController
        Inherits System.Web.Http.ApiController

        Private db As New RestaurantSMKEntities

        Function GetDetailOrder()
            Dim detailOrder = From m In db.DetailOrders
                              Where m.Status = "Deliver"
                              Select m.DetailId, m.MsMenu.Name, m.MsMenu.Photo, m.Qty, m.Status, m.HeaderOrder.TableId
            Return New With {.detail = detailOrder}
        End Function

        ' GET: api/DetailOrders/5
        Function GetDetailOrder(ByVal Orderid As String) As IHttpActionResult
            Dim detailOrder = From m In db.DetailOrders
                              Where m.OrderId = Orderid And m.Status = "Deliver"
                              Select m.DetailId, m.MsMenu.Name, m.MsMenu.Photo, m.Qty, m.Status
            If IsNothing(detailOrder) Then
                Return NotFound()
            End If
            Dim res = New With {
                .OrderId = Orderid,
                .detail = detailOrder
            }
            Return Ok(res)
        End Function
        <Route("api/Delivered")>
        Function UpdateStatus(ByVal idDetail As String)
            Dim res = (From m In db.DetailOrders
                       Where m.DetailId = idDetail
                       Select m).SingleOrDefault
            res.Status = "Delivered"
            If db.SaveChanges() > 0 Then
                Return Me.Request.CreateResponse(HttpStatusCode.OK, New With {
                    .Message = "Sukses"})
            End If
            Return Me.Request.CreateResponse(HttpStatusCode.BadRequest, New With {
                    .Message = "Gagal"})
        End Function

        ' PUT: api/DetailOrders/5
        <ResponseType(GetType(Void))>
        Function PutDetailOrder(ByVal id As Integer, ByVal detailOrder As DetailOrder) As IHttpActionResult
            If Not ModelState.IsValid Then
                Return BadRequest(ModelState)
            End If

            If Not id = detailOrder.DetailId Then
                Return BadRequest()
            End If

            db.Entry(detailOrder).State = EntityState.Modified

            Try
                db.SaveChanges()
            Catch ex As DbUpdateConcurrencyException
                If Not (DetailOrderExists(id)) Then
                    Return NotFound()
                Else
                    Throw
                End If
            End Try

            Return StatusCode(HttpStatusCode.NoContent)
        End Function

        ' POST: api/DetailOrders
        <ResponseType(GetType(DetailOrder))>
        Function PostDetailOrder(ByVal detailOrder As DetailOrder) As IHttpActionResult
            If Not ModelState.IsValid Then
                Return BadRequest(ModelState)
            End If

            db.DetailOrders.Add(detailOrder)
            db.SaveChanges()

            Return CreatedAtRoute("DefaultApi", New With {.id = detailOrder.DetailId}, detailOrder)
        End Function

        ' DELETE: api/DetailOrders/5
        <ResponseType(GetType(DetailOrder))>
        Function DeleteDetailOrder(ByVal id As Integer) As IHttpActionResult
            Dim detailOrder As DetailOrder = db.DetailOrders.Find(id)
            If IsNothing(detailOrder) Then
                Return NotFound()
            End If

            db.DetailOrders.Remove(detailOrder)
            db.SaveChanges()

            Return Ok(detailOrder)
        End Function

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If (disposing) Then
                db.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

        Private Function DetailOrderExists(ByVal id As Integer) As Boolean
            Return db.DetailOrders.Count(Function(e) e.DetailId = id) > 0
        End Function
    End Class
End Namespace