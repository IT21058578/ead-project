import React, {useState}from 'react'
import Swal from 'sweetalert2';
import { useDeleteUserMutation, useGetAllUsersQuery } from '../../store/apiquery/usersApiSlice';
import Spinner from '../Spinner';
import { UserType } from '../../types';
import { useAppSelector } from '../../hooks/redux-hooks'

/**
 * ListOfCustomers component
 * 
 * This component renders a table of customers with the following columns:
 * - No.
 * - FirstName
 * - LastName
 * - Email
 * - Action (Delete)
 * 
 * The component also renders a search input field that allows users to search for customers by name, price, or total stock.
 * 
 * The component also renders a spinner if the customers list is loading.
 * 
 * @returns {React.ReactNode} - a React node that renders the customers table or a spinner if the customers list is loading.
 */
const ListOfCustomers = () => {

  let count = 0;

  const { isLoading, data: customersList, isSuccess, isError } = useGetAllUsersQuery('api/users');
  const [deleteCustomer, deletedResult] = useDeleteUserMutation();

  /**
   * Deletes a customer with the given id.
   * It shows a confirmation dialog with a warning icon and asks the user if they are sure to delete the customer.
   * If the user confirms, it calls the deleteCustomer function with the given id.
   * @param {string} id - the id of the customer to delete
   */
  const deleteItem = (id: string) => {
    Swal.fire({
      title: 'Are you sure?',
      text: "Are you sure to delete this customer ?",
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Yes, delete it!'
    }).then((r) => {
      if (r.isConfirmed) {
        deleteCustomer(id);
      }
    });
  }

  // search bar coding 
  const [searchInput, setSearchInput] = useState<string>('');

  let data: React.ReactNode;

    // Filter customers based on the search input
  const filteredCustomers = customersList?.data.filter((user: UserType) =>{
    const firstname = user.firstName?.toLowerCase();
    const lastname = user.lastName?.toLowerCase();
    const email = user.email?.toLowerCase();
    const search = searchInput.toLowerCase();

    return (
      firstname?.includes(search) ||
      lastname?.includes(search) ||
      email?.includes(search) 
    );
  });

  data = isLoading || isError
    ? null
    : isSuccess && customersList && customersList.data
      ? filteredCustomers.map((customer: UserType) => {
        const customerId = customer.id || '';

        return (
          <tr className="p-3" key={customer.id}>
            <td scope="row w-25">{++count}</td>
            <td className='fw-bold'>{customer.firstName}</td>
            <td className='fw-bold'>{customer.lastName}</td>
            <td>{customer.email}</td>
            <td className='fw-bold d-flex gap-2 justify-content-center'>
              <a href="#" className='p-2 rounded-2 bg-danger' title='Delete' onClick={(e) => {
                e.preventDefault();
                deleteItem(customerId)
              }}><i className="bi bi-trash"></i></a>
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
        placeholder="Search Review"
        value={searchInput}
        onChange={(e) => setSearchInput(e.target.value)}
        className="form-control w-100 p-2"
      />
    </div>
      <div className="table-responsive">
        <table className="table table-default text-center table-bordered">
          <thead>
            <tr className='fd-bg-primary text-white'>
              <th scope="col" className='p-3'>No</th>
              <th scope="col" className='p-3'>FIRSTNAME</th>
              <th scope="col" className='p-3'>LASTNAME</th>
              <th scope="col" className='p-3'>EMAIL</th>
              <th scope="col" className='p-3'>ACTION</th>
            </tr>
          </thead>
          <tbody>
            {
              data
            }
          </tbody>
        </table>
        </div>
      </div>) : ( <Spinner />
  );
}

/**
 * CustomersMain component
 * 
 * This component renders a list of customers
 * 
 * The component returns a JSX node that renders a table of customers
 * with the following columns: No., FIRSTNAME, LASTNAME, EMAIL, ACTION
 * 
 * The component also renders a search input field that allows users to search for customers by name, email, etc.
 * 
 * @returns {JSX.Element} - a JSX node that renders the customers table
 */
const CustomersMain = () => {
  return (
    <div className='text-black'>
      <h4 className="fw-bold mb-5">List of Customers</h4>
      <div className="subPartMain">
        <ListOfCustomers />
      </div>
    </div>
  )
}

export default CustomersMain