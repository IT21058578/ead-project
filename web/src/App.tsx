import { Route, Routes } from "react-router-dom"
import ForgotPassword from "./views/ForgotPassword"
import Login from "./views/Login"
import PageNotFound from "./views/PageNotFound"
import AdminPanel from "./views/Admin/AdminPanel"
import DashMain from "./components/Admin/DashMain"
import ProductMain from "./components/Admin/ProductMain"
import RoutePaths from "./config"
import CategoryMain from "./components/Admin/OrdersMain"
import CustomersMain from "./components/Admin/CustomersMain"
import AdminAccount from "./components/Admin/AdminAccount"
import PrivateRoute from "./components/PrivateRoute"
import RedirectIfAuthenticate from "./components/RedirectIfAuthenticate"
import ReviewMain from "./components/Admin/ReviewMain"
import VendorsMain from "./components/Admin/VendorsMain"

function App() {

  return (
    <Routes>

      {/* <Route path={RoutePaths.home} element={<Home />}></Route> */}
      {/* <Route path={RoutePaths.shop} element={<Shop />}></Route>
      <Route path={RoutePaths.wishlist} element={<WishList />}></Route>
      <Route path={RoutePaths.blog} element={<BlogPage />}></Route>
      <Route path={RoutePaths.post} element={<PostView />}></Route>
      <Route path={RoutePaths.shopping} element={<ShoppingCart />}></Route>
      <Route path={RoutePaths.contact} element={<ContactUs />}></Route>
      <Route path={RoutePaths.team} element={<TeamMembers />}></Route> */}
      <Route element={<RedirectIfAuthenticate />} >
        <Route path={RoutePaths.login} element={<Login />}></Route>
        {/* <Route path={RoutePaths.signup} element={<SignUp />}></Route> */}
      </Route>
      <Route path={RoutePaths.passwordReset} element={<ForgotPassword />}></Route>

      {/* ADMINS ROUTES */}

      <Route element={<PrivateRoute type={0} />} >
        <Route path={RoutePaths.admin} element={<AdminPanel  currentComponent={<DashMain />} />}></Route>
        <Route path={RoutePaths.adminProducts} element={<AdminPanel  currentComponent={<ProductMain />} />}></Route>
        {/* <Route path={RoutePaths.adminSlides} element={<AdminPanel  currentComponent={<SlidesMain />} />}></Route> */}
        <Route path={RoutePaths.adminCategories} element={<AdminPanel  currentComponent={<CategoryMain />} />}></Route>
        <Route path={RoutePaths.adminReview} element={<AdminPanel  currentComponent={<ReviewMain />} />}></Route>
        <Route path={RoutePaths.adminCustomers} element={<AdminPanel  currentComponent={<CustomersMain />} />}></Route>
        <Route path={RoutePaths.adminVendors} element={<AdminPanel  currentComponent={<VendorsMain />} />}></Route>
      </Route>

      <Route path="*" element={<PageNotFound />}></Route>

    </Routes>
    
  )
}

export default App
