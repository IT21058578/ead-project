import React from "react";
// import { useGetStartisticsQuery } from "../../store/apiquery/usersApiSlice";
import Spinner from "../Spinner";
import { useGetAllProductsQuery } from "../../store/apiquery/productApiSlice";
import { useGetAllUsersQuery } from "../../store/apiquery/usersApiSlice";
import { useGetAllOrderQuery } from "../../store/apiquery/OrderApiSlice";
import { useGetAllReviewQuery } from "../../store/apiquery/ReviewApiSlice";


/**
 * The main component for the admin dashboard
 * @function
 * @returns {ReactElement} The component
 * @example
 *   <DashMain />
 */
const DashMain = () => {
  // const { data: starts, isLoading } = useGetStartisticsQuery("api/users");
  const {data: products } = useGetAllProductsQuery("api/products");
  const {data: reviews  } = useGetAllReviewQuery("api/reviews");
  const {data: users } = useGetAllUsersQuery("api/users");
  const {data: orders } = useGetAllOrderQuery("api/orders");


  /**
   * Redirects to the product reports download page
   * @function
   */
  const handleProductsDownload = () => {
      const reportURL = 'http://localhost:3000/products/reports'; 
      window.location.href = reportURL;
  };

  /**
   * Redirects to the order reports download page
   * @function
   */
  const handleOrdersDownload = () => {
    const reportURL = 'http://localhost:3000/orders/reports'; 
    window.location.href = reportURL;
  };

  /**
   * Redirects to the customer reports download page
   * @function
   */
  const handleCustomersDownload = () => {
    const reportURL = 'http://localhost:3000/users/reports'; 
    window.location.href = reportURL;
  };


  /**
   * Formats a number or string by padding it with zeros to the given length
   * @param {number|string} nb - the number or string to format
   * @param {number} [lenght=2] - the length of the formatted string
   * @returns {string} the formatted string
   * @example
   *   format(1) // returns "01"
   *   format(1, 3) // returns "001"
   *   format("1") // returns "01"
   */
  const format = (nb: number | string, lenght: number = 2) => {
    const value = nb.toString();
    return value.padStart(lenght, "0");
  };

  return (
    <div className="dash-user pt-2">
      <h4 className="text-dark fw-bold">Dashboard</h4>
        <div className="resume d-grid grid-4 gap-3 fw-bold mt-3">
        <div className="r-card d-flex flex-column align-items-center gap-3 border border-1 bg-secondary p-3">
          <h1>{products?.meta.total}</h1>
          <h4 className="align-self-center">Total Products</h4>
        </div>
        <div className="r-card d-flex flex-column align-items-center gap-3 border border-1 bg-secondary p-3">
          <h1>{reviews?.meta.total}</h1>
          <h4 className="align-self-center">Total Reviews</h4>
        </div>
        <div className="r-card d-flex flex-column align-items-center gap-3 border border-1 bg-secondary p-3">
          <h1>{users?.meta.total}</h1>
          <h4 className="align-self-center">Total Users</h4>
        </div>
        <div className="r-card d-flex flex-column align-items-center gap-3 border border-1 bg-secondary p-3">
          <h1>{orders?.meta.total}</h1>
          <h4 className="align-self-center">Total Orders</h4>
        </div>
      </div>
      <h4></h4>
      <h4></h4>
      <h4 className="text-dark fw-bold">Reports</h4>
        <div className="resume d-grid grid-4 gap-3 fw-bold mt-3">
          <button className="r-card btn btn-success d-flex justify-content-center gap-3 border border-1 p-3" onClick={handleProductsDownload}>
            <h4 className="align-self-center">Products</h4>
          </button>
          <button className="r-card btn btn-success d-flex justify-content-center gap-3 border border-1 p-3" >
            <h4 className="align-self-center">Reviews</h4>
          </button>
          <button className="r-card btn btn-success d-flex justify-content-center gap-3 border border-1 p-3" onClick={handleCustomersDownload}>
            <h4 className="align-self-center">Users</h4>
          </button>
          <button className="r-card btn btn-success d-flex justify-content-center gap-3 border border-1 p-3" onClick={handleOrdersDownload}>
            <h4 className="align-self-center">Ordes</h4>
          </button>
        </div>

      {/* <div className="user-dashboard p-3 border border-3 text-black mt-5">
        <p className="opacity-75">
          From your admin dashboard you can view your popular products, manage
          your account and others, and edit your password and account details
        </p>
      </div> */}
    </div>
  );
};

export default DashMain;
