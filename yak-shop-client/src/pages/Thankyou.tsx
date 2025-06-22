import React from "react";
import { useLocation, useNavigate } from "react-router-dom";
import { OrderResponse } from "../services/api";
import { OrderRequest } from "../services/api";

const ThankYou: React.FC = () => {
  const location = useLocation();
  const navigate = useNavigate();
  const response: OrderResponse | null = location.state?.response ?? null; // result from backend (could be null)
  const order: OrderRequest | undefined = location.state?.order; // original order submitted by user

  let message = "";

  if (response === null) {
    message = "Unfortunately, we could not fulfill your order.";
  } else if (
    response.milk === order?.order.milk &&
    response.skins === order?.order.skins
  ) {
    message = "Your order was fully completed. Thank you for shopping with us!";
  } else {
    message =
      "Your order was partially completed. The available items will be delivered.";
  }

  return (
    <div className="p-4 max-w-xl mx-auto text-center">
      {/* Page title */}
      <h1 className="text-2xl font-bold mb-4">Thank you for your order!</h1>

      {/* Display order status message */}
      <p className="mb-6">{message}</p>

      {/* Show order details if available */}
      {response && (
        <div className="mb-6 text-sm text-gray-700">
          <p>
            Ordered by: <strong>{order?.customer}</strong>
          </p>
          {response.milk !== undefined && <p>Milk: {response.milk} liter(s)</p>}
          {response.skins !== undefined && <p>Skins: {response.skins}</p>}
        </div>
      )}

      {/* Button to return to the order page */}
      <button
        onClick={() => navigate("/order")}
        className="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700"
      >
        Place another order
      </button>
    </div>
  );
};

export default ThankYou;
