Public Class BundleOperation
    Public Shared Sub RegisterBundles(bundles As System.Web.Optimization.BundleCollection)
        bundles.Add(New System.Web.Optimization.StyleBundle("~/Styles/CSS").Include(
                    "~/Styles/StyleSheet.css", "~/Scripts/san_indicator/san_indicator.css"))

        bundles.Add(New System.Web.Optimization.ScriptBundle("~/Scripts/JS").Include(
                    "~/Scripts/jquery-3.6.0.min.js", "~/Scripts/activity-indicator/jquery.activity-indicator-1.0.0.min.js",
                    "~/Scripts/san_indicator/jquery.san_indicator.js"))
    End Sub
End Class
