export interface SalesData {
  month: string;
  sales: number;
  revenue: number;
}

export interface PerformanceMetric {
  id: string;
  title: string;
  value: number;
  change: number;
  trend: "up" | "down" | "stable";
}

export interface RevenueData {
  category: string;
  amount: number;
  percentage: number;
}

export const salesData: SalesData[] = [
  { month: "Jan", sales: 4000, revenue: 240000 },
  { month: "Feb", sales: 3000, revenue: 180000 },
  { month: "Mar", sales: 5000, revenue: 300000 },
  { month: "Apr", sales: 4500, revenue: 270000 },
  { month: "May", sales: 6000, revenue: 360000 },
  { month: "Jun", sales: 5500, revenue: 330000 },
  { month: "Jul", sales: 7000, revenue: 420000 },
  { month: "Aug", sales: 6500, revenue: 390000 },
  { month: "Sep", sales: 8000, revenue: 480000 },
  { month: "Oct", sales: 7500, revenue: 450000 },
  { month: "Nov", sales: 9000, revenue: 540000 },
  { month: "Dec", sales: 8500, revenue: 510000 },
];

export const performanceMetrics: PerformanceMetric[] = [
  {
    id: "total-revenue",
    title: "Total Revenue",
    value: 4260000,
    change: 12.5,
    trend: "up",
  },
  {
    id: "total-sales",
    title: "Total Sales",
    value: 71000,
    change: 8.2,
    trend: "up",
  },
  {
    id: "avg-order-value",
    title: "Average Order Value",
    value: 60,
    change: -2.1,
    trend: "down",
  },
  {
    id: "conversion-rate",
    title: "Conversion Rate",
    value: 3.2,
    change: 0.5,
    trend: "up",
  },
];

export const revenueByCategory: RevenueData[] = [
  { category: "Products", amount: 2556000, percentage: 60 },
  { category: "Services", amount: 1278000, percentage: 30 },
  { category: "Subscriptions", amount: 426000, percentage: 10 },
];

export const topProducts = [
  { name: "Enterprise Suite", sales: 1200, revenue: 360000 },
  { name: "Professional Plan", sales: 800, revenue: 240000 },
  { name: "Starter Package", sales: 1500, revenue: 150000 },
  { name: "Premium Add-ons", sales: 600, revenue: 180000 },
  { name: "Custom Solutions", sales: 300, revenue: 450000 },
];

export const recentActivities = [
  {
    id: "1",
    type: "sale",
    message: "New sale: Enterprise Suite - $300",
    time: "2 minutes ago",
  },
  {
    id: "2",
    type: "user",
    message: "New user registration: john.doe@company.com",
    time: "5 minutes ago",
  },
  {
    id: "3",
    type: "revenue",
    message: "Monthly revenue target achieved!",
    time: "1 hour ago",
  },
  {
    id: "4",
    type: "sale",
    message: "New sale: Professional Plan - $150",
    time: "2 hours ago",
  },
  {
    id: "5",
    type: "alert",
    message: "Low inventory alert: Starter Package",
    time: "3 hours ago",
  },
];
