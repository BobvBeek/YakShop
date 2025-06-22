import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import {
  getCurrentStock, // API call to fetch current stock from the backend
  placeOrder, // API call to submit a new order
  OrderRequest, // Type definition for the order input
  OrderResponse, // Type definition for the server's response to an order
  Stock, // Type definition for stock data
} from "../services/api";

const Order: React.FC = () => {
  // Local state for stock and form fields
  const [stock, setStock] = useState<Stock | null>(null);
  const [milk, setMilk] = useState(0);
  const [skins, setSkins] = useState(0);
  const [customer, setCustomer] = useState("");
  const navigate = useNavigate();

  // Fetch stock from backend on initial render and every 10 seconds after
  useEffect(() => {
    const fetchStock = async () => {
      try {
        const data = await getCurrentStock();
        setStock(data); // update state with fresh stock info
      } catch (error) {
        console.error("Failed to fetch stock", error);
      }
    };

    fetchStock();
    const interval = setInterval(fetchStock, 10000); // refresh every 10 seconds
    return () => clearInterval(interval);
  }, []);

  // Handles the form submission
  const handleOrder = async () => {
    const order: OrderRequest = {
      customer,
      order: {
        milk,
        skins,
      },
    };

    try {
      const response: OrderResponse = await placeOrder(order);
      // Navigate to thank you page with the order and response
      navigate("/thankyou", { state: { response, order } });
    } catch (error) {
      console.error("Order failed", error);
      // Still navigate to thank you page even if it failed, with null response
      navigate("/thankyou", { state: { response: null, order } });
    }
  };

  return (
    <div className="p-4 max-w-xl mx-auto">
      <h1 className="text-2xl font-bold mb-4">Yak Shop - Order</h1>

      {/* Show current stock info or loading message */}
      {stock ? (
        <div className="mb-6">
          <p className="text-lg mb-2">
            Current day: <strong>{stock.day}</strong>
          </p>
          <table className="table-auto border w-full">
            <thead>
              <tr>
                <th className="border px-4 py-2">Milk (liters)</th>
                <th className="border px-4 py-2">Skins</th>
              </tr>
            </thead>
            <tbody>
              <tr>
                <td className="border px-4 py-2">{stock.milk.toFixed(2)}</td>
                <td className="border px-4 py-2">{stock.skins}</td>
              </tr>
            </tbody>
          </table>
        </div>
      ) : (
        <p>Loading stock...</p>
      )}

      {/* Input field for customer name */}
      <div className="mb-6">
        <label className="block mb-4 font-semibold">Your name</label>
        <input
          type="text"
          value={customer}
          onChange={(e) => setCustomer(e.target.value)}
          style={{ marginTop: "12px" }}
          className="border p-2 w-full"
        />
      </div>

      {/* Inputs for milk and skin order quantities */}
      <div className="grid grid-cols-2 gap-4 mb-6">
        <div>
          <label className="block mb-4 font-semibold">Milk (liters)</label>
          <input
            type="number"
            value={milk}
            onChange={(e) => setMilk(Number(e.target.value))}
            min={0}
            style={{ marginTop: "12px" }}
            className="border p-2 w-full"
          />
        </div>
        <div>
          <label className="block mb-4 font-semibold">Skins</label>
          <input
            type="number"
            value={skins}
            onChange={(e) => setSkins(Number(e.target.value))}
            min={0}
            style={{ marginTop: "12px" }}
            className="border p-2 w-full"
          />
        </div>
      </div>

      {/* Submit button to place the order */}
      <button
        onClick={handleOrder}
        className="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700"
      >
        Place Order
      </button>
    </div>
  );
};

export default Order;
