import { Routes, Route } from "react-router-dom";
import Home from "./pages/Home";
import Order from "./pages/Order";
import Thanks from "./pages/Thankyou";

function App() {
  return (
    <Routes>
      <Route path="/" element={<Home />} />
      <Route path="/order" element={<Order />} />
      <Route path="/thankyou" element={<Thanks />} />
    </Routes>
  );
}

export default App;
