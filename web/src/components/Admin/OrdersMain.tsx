import React, { useState, SyntheticEvent, useEffect } from 'react'
import Swal from 'sweetalert2';
import Spinner from '../Spinner';
import { HandleResult } from '../HandleResult';
import { Order } from '../../types';
import { useUpdateOrderStatusMutation } from '../../store/apiquery/OrderApiSlice';
import { useDeleteOrderMutation } from '../../store/apiquery/OrderApiSlice';
import { useGetAllOrderQuery } from '../../store/apiquery/OrderApiSlice';
import { ToastContainer, toast } from "react-toastify";
import { getItem } from '../../Utils/Generals';


/**
 * @function UpdateOrders
 * @description Form to update the status of an order
 * @param {Order} Orders - The order to be updated
 * @returns {JSX.Element} - A form with a select for the order status
 */
const UpdateOrders = ({Orders}: {Orders : Order}) => {

  const userRole = getItem('userRole');

	const [updateData, setUpdateData] = useState(Orders);
	const [updateOrders, udpateResult] = useUpdateOrderStatusMutation();

  const orderID = Orders.id;

  const [formData, setFormData] = useState({
    // status: updateData.status,
    userId: updateData.userId,
    vendorId: updateData.vendorId,
    status: updateData.status,
    products: updateData.products,
    deliveryNote: updateData.deliveryNote,
    deliveryAddress: updateData.deliveryAddress,
    deliveryDate: updateData.deliveryDate,
  });

  /**
   * Handles the change event for the form fields.
   * Updates the formData state with the changed value.
   * @param e - The change event.
   */
  const handleUpdateValue = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement>) => {
    const { name, value } = e.target;
    setFormData({ ...formData, [name]: value });
  };

  const status = formData.status?.toString();

  /**
   * Handles the submission of the form. Calls the updateOrders mutation and
   * displays a toast notification based on the result.
   * @param e - The form submission event.
   * @returns {Promise<void>}
   */
  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    try {
      const result = await updateOrders({orderID, formData});

      if ('data' in result && result.data) {
        console.log('Order States Updated successfully');
        toast.success("Order States Updated successfully");
        // setFormData({ status:''});
      } else if ('error' in result && result.error) {
        console.error('Order States Update failed', result.error);
        toast.error("Order States Update failed");
      }
    } catch (error) {
      console.error('Order States Update failed`', error);
      toast.error("Order States Update failed");
    }
  };
  

	return (
		<form action="" method="patch" className="checkout-service p-3" onSubmit={handleSubmit}>
			<input type="hidden" name="id" value={updateData.id} />
      <div>
          <label className='w-100'>
            <span>Order Status</span>
            <select name="status" value={formData.status} className="form-control w-100 rounded-0 p-2" onChange={handleUpdateValue}>
              <option value="Pending">Pending</option>
              <option value="PartiallyDelivered">Partially Delivered</option>
              <option value="Delivered">Delivered</option>
              <option hidden={userRole === "Vendor"} value="Completed">Completed</option>
              <option hidden={userRole === "Vendor"} value="Cancelled">Cancelled</option>
            </select>
          </label>
        </div>
        <div className="mt-4">
          <ToastContainer/>
        </div>
			<div className='mt-3'>{udpateResult.isLoading ?
				<button className="fd-btn w-25 text-center border-0"><span className="spinner-grow spinner-grow-sm" role="status" aria-hidden="true"></span>
					Loading...</button> :
				<button className="fd-btn w-25 text-center border-0" type='submit'>UPDATE Orders status</button>
			}</div>
		</form>
	)


}

/**
 * ListOfOrders component
 * 
 * This component renders a table of orders with the following columns:
 * - Order No.
 * - Items
 * - Total
 * - Order Status
 * - Manage (Edit, Delete)
 * 
 * The component also renders a search input field that allows users to search for orders by name, price, or total stock.
 * 
 * The component also renders a spinner if the orders list is loading.
 * 
 * @param {{ setOrders: Function, setPage: Function }} props - an object with two properties: setOrders and setPage.
 * @returns {React.ReactNode} - a React node that renders the orders table or a spinner if the orders list is loading.
 */
const ListOfOrders = ({ setOrders, setPage }: { setOrders: Function, setPage: Function }) => {

  /**
   * Sets the current orders to the given orders and changes the page to 'add'.
   * @param {Order} Orders - the orders to set as the current orders
   */
  const parseOrders = (Orders: Order) => {
    setOrders(Orders);
    setPage('add');
  }
  const { isLoading, data: OrdersList, isSuccess, isError } = useGetAllOrderQuery('api/orders');
  const [deleteOrders, deletedResult] = useDeleteOrderMutation();


  /**
   * Deletes an order with the given id.
   * It shows a confirmation dialog with a warning icon and asks the user if they are sure to delete the order.
   * If the user confirms, it calls the deleteOrders function with the given id.
   * @param {string} id - the id of the order to delete
   */
  const deleteItem = (id: string) => {
    Swal.fire({
      title: 'Are you sure?',
      text: "Are you sure to delete this Order ?",
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Yes, delete it!'
    }).then((r) => {
      if (r.isConfirmed) {
        deleteOrders(id);
      }
    });
  }

  // search bar coding 
  const [searchInput, setSearchInput] = useState<string>('');

  let data: React.ReactNode;
  let count = 0;

  // Filter products based on the search input
  const filteredOrders = OrdersList?.data.filter((order: Order) =>
    order.status?.toLowerCase().includes(searchInput.toLowerCase())
  );

  data = isLoading || isError
    ? null
    : isSuccess
      ? filteredOrders.map((Orders: Order) => {
        const totalPrice = Object.values(Orders.products).reduce((acc, product) => {
          return acc + product.price * product.quantity;
        }, 0);
        const orderID = Orders.id;

        return (
          <tr className="p-3" key={Orders.id}>
            <td scope="row w-25">{Orders.id}</td>
            <td className='fw-bold'>
                        <ul>
                            {Object.entries(Orders?.products).map(([productId, item]) => (
                            <li key={productId}>
                                Price: {item.price}, Qty: {item.quantity}
                            </li>
                            ))}
                        </ul>
            </td>
            <td>{totalPrice}</td>
            <td className="col-2">{Orders?.deliveryAddress}</td>
            <td className="col-2">{Orders?.deliveryNote}</td>
            <td>
                  <span style={{ color: Orders.status === 'Completed' ? 'green' : 'red' }}>
                                {Orders?.status}
                  </span>
            </td>
            <td className='fw-bold d-flex gap-2 justify-content-center'>
              <a href="#" className='p-2 rounded-2 bg-secondary' onClick={(e) => parseOrders(Orders)} title='Edit'><i className="bi bi-pen"></i></a>
              {/* <a href="#" className='p-2 rounded-2 bg-danger' title='Delete' onClick={(e) => deleteItem(Orders.id)}><i className="bi bi-trash"></i></a> */}
            </td>
          </tr>
        )
      })
      : null;

  return !isLoading ? (
      <div>
     {/* Add a search input field */}
     <div className="mb-3">
      <input
        type="text"
        placeholder="Search Orders"
        value={searchInput}
        onChange={(e) => setSearchInput(e.target.value)}
        className="form-control w-100 p-2"
      />
    </div>

    <div className="table-responsive">
      <table className="table table-default text-center table-bordered">
        <thead>
          <tr className='fd-bg-primary text-white'>
            <th scope="col" className='p-3'>Order No.</th>
            <th scope="col" className='p-3'>ITEMS</th>
            <th scope="col" className='p-3'>TOTAL</th>
            <th scope="col" className='p-3'>DELIVERY ADDRESS</th>
            <th scope="col" className='p-3'>DELIVERY NOTE</th>
            <th scope="col" className='p-3'>ORDER STATUS</th>
            <th scope="col">MANAGE</th>
          </tr>
        </thead>
        <tbody>
          {
            data
          }
        </tbody>
      </table>
      </div>
    </div>) : (<Spinner />
  );
}

/**
 * The main component for the Orders page. This component renders a list of orders
 * and provides the ability to view an order report or add a new order.
 *
 * The component state is managed by the `useState` hook, which is used to store the
 * current page and the order to be edited.
 *
 * The component renders a list of orders if the current page is "list", or it renders
 * the form to add or edit an order if the current page is "add".
 *
 * The component also renders a button to switch between the list and add/edit pages.
 */
const OrdersMain = () => {
  const [page, setPage] = useState('list');
  const [currentOrder, setCurrentOrder] = useState(null);

  const changeToList = () => { setPage('add'); setCurrentOrder(null) }
  const changeToAdd = () => { setPage('list'); }

  return (
    <div className='text-black'>
      <h4 className="fw-bold">Orders</h4>
      <div className="add-product my-3 d-flex justify-content-end">
				{
					// page === 'list' ?
					// 	<a href="#" className="fd-btn bg-secondary w-25 text-center rounded-3" onClick={changeToList}>ORDER REPORS</a> :
						<a href="#" className="fd-btn bg-secondary w-25 text-center rounded-3" onClick={changeToAdd}>ORDER LIST</a>
				}
			</div>
			<div className="subPartMain">
      {page === 'list' ? (
  <ListOfOrders setOrders={setCurrentOrder} setPage={setPage} />
    ) : currentOrder ? ( // Check if currentOrder is not null
      <UpdateOrders Orders={currentOrder} />
    ) : null}
			</div>
    </div>
  )
}

export default OrdersMain