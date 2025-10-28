using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DashboardSummaryViewModel
    {
        // Các số liệu chính
        public decimal TodayRevenue { get; set; }
        public int NewOrdersCount { get; set; }
        public int NewCustomersCount { get; set; }
        public int ConfirmedReservationsCount { get; set; }

        // Báo cáo thay đổi
        public decimal RevenueChangePercent { get; set; } // Ví dụ: +12.5%
        public string RevenueChangeStatus { get; set; } // Ví dụ: "so với hôm qua"

        // Danh sách Món ăn bán chạy
        public List<TopProductDTO> TopSellingProducts { get; set; } = new List<TopProductDTO>();

        // Danh sách Đơn hàng gần đây
        public List<OrderSummaryDTO> RecentOrders { get; set; } = new List<OrderSummaryDTO>();
    }

    // DTO cho các mục bán chạy
    public class TopProductDTO
    {
        public string ProductName { get; set; }
        public int UnitsSold { get; set; }
    }

    // DTO cho Tóm tắt đơn hàng
    public class OrderSummaryDTO
    {
        public string OrderId { get; set; }
        public string CustomerName { get; set; }
        public string OrderTime { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } // Hoàn thành, Đang xử lý, Đã hủy
    }
}