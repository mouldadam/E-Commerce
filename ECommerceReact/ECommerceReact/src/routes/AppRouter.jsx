import { Routes, Route } from "react-router-dom";
import Home from "../pages/Home.jsx";
import { ROUTES } from "../Utility/constants.js";
import Login from "../pages/auth/Login.jsx";
import Register from "../pages/auth/Register.jsx";
import OrderManagement from "../pages/order/OrderManagement.jsx";
import MenuItemManagement from "../pages/menu/MenutItemManagement.jsx";
import Cart from "../pages/cart/Cart.jsx";
import Checkout from "../pages/cart/Checkout.jsx";
import OrderConfirmation from "../pages/order/OrderConfirmation.jsx";
function AppRoutes() {
  return (
    <Routes>
      <Route path={ROUTES.HOME} element={<Home />} />
      <Route path={ROUTES.LOGIN} element={<Login />} />
      <Route path={ROUTES.REGISTER} element={<Register />} />
      <Route path={ROUTES.ORDER_MANAGEMENT} element={<OrderManagement />} />
      <Route path={ROUTES.MENU_MANAGEMENT} element={<MenuItemManagement />} />
      <Route path={ROUTES.CART} element={<Cart />} />
      <Route path={ROUTES.CHECKOUT} element={<Checkout />} />
      <Route path={ROUTES.ORDER_CONFRIMATION} element={<OrderConfirmation />} />
    </Routes>
  );
}

export default AppRoutes;
