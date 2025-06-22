import React, { useEffect, useState } from "react";
import { getOriginalHerd, Yak } from "../services/api";
import { useNavigate } from "react-router-dom";

const Home: React.FC = () => {
  const navigate = useNavigate();
  const [herd, setHerd] = useState<Yak[]>([]); // stores the original yak herd from backend

  // Fetch the herd once when the page loads
  useEffect(() => {
    const fetchHerd = async () => {
      try {
        const data = await getOriginalHerd();
        setHerd(data); // update state with herd
      } catch (error) {
        console.error("Failed to fetch herd", error);
      }
    };
    fetchHerd();
  }, []);

  return (
    <div className="flex flex-col items-center justify-center min-h-screen text-center px-4 py-8">
      {/* Yak shop logo image */}
      <img
        src={require("./yak-logo.png")}
        alt="Yak Logo"
        className="w-[120px] h-auto mb-4"
      />

      {/* Render visual yak herd with colored blocks (red for females, blue for males) */}
      <div
        style={{
          display: "flex",
          gap: "12px",
          flexWrap: "wrap",
          justifyContent: "flex-start",
          marginBottom: "1.5rem",
        }}
      >
        {herd.map((yak) => (
          <div
            key={yak.name}
            style={{
              border: "2px solid black",
              padding: "8px 12px",
              borderRadius: "6px",
              backgroundColor: yak.sex === "FEMALE" ? "#f87171" : "#60a5fa",
              color: "white",
              minWidth: "80px",
              textAlign: "center",
              fontSize: "0.9rem",
            }}
          >
            {yak.name}
          </div>
        ))}
      </div>

      {/* Welcome heading and description */}
      <h1 className="text-3xl font-bold mb-4">Welcome to the Yak Shop</h1>
      <p className="mb-6 text-lg">
        Order fresh yak milk and high-quality hides directly from the tundra.
      </p>

      {/* Navigation button to the order page */}
      <button
        onClick={() => navigate("/order")}
        className="bg-green-600 text-white px-6 py-3 rounded-xl hover:bg-green-700 mb-8"
      >
        Start Shopping
      </button>
    </div>
  );
};

export default Home;
