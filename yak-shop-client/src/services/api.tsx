// Interfaces (data types)
export interface Yak {
  name: string;
  age: number;
  sex: string;
}

export interface Herd {
  herd: Yak[];
}

export interface Stock {
  milk: number;
  skins: number;
  day?: number;
  lastUpdate?: string;
}

export interface OrderRequest {
  customer: string;
  order: {
    milk: number;
    skins: number;
  };
}

export interface OrderResponse {
  milk?: number;
  skins?: number;
}

// Config
const baseUrl = "https://localhost:7075";

// API-endpoint functions

// Load a new herd of yaks
export async function loadYaks(herd: Herd): Promise<void> {
  const res = await fetch(`${baseUrl}/yaks/load`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(herd),
  });
  if (res.status !== 205) {
    throw new Error("Failed to load yaks");
  }
}

// Get stock at a specific day
export async function getStockAtDay(day: number): Promise<Stock> {
  const res = await fetch(`${baseUrl}/stock/${day}`);
  if (!res.ok) throw new Error("Failed to fetch stock for day " + day);
  return res.json();
}

// Get current stock (no day param)
export async function getCurrentStock(): Promise<Stock> {
  const res = await fetch(`${baseUrl}/stock`);
  if (!res.ok) throw new Error("Failed to fetch current stock");
  return res.json();
}

// Get herd state at a specific day
export async function getHerdAtDay(day: number): Promise<Yak[]> {
  const res = await fetch(`${baseUrl}/herd/${day}`);
  if (!res.ok) throw new Error("Failed to fetch herd for day " + day);
  const json = await res.json();
  return json.herd;
}

// Get entire herd
export async function getOriginalHerd(): Promise<Yak[]> {
  const res = await fetch(`${baseUrl}/herd`);
  if (!res.ok) throw new Error("Failed to fetch herd");
  const json = await res.json();
  return json.herd;
}

// Place an order
export async function placeOrder(order: OrderRequest): Promise<OrderResponse> {
  const res = await fetch(`${baseUrl}/order`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(order),
  });

  if (res.status === 201 || res.status === 206) {
    return res.json();
  }

  if (res.status === 404) {
    throw new Error("Out of stock");
  }

  throw new Error(`Unexpected response: ${res.status}`);
}
