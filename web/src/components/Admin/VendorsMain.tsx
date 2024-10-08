import React, { useState, useEffect, useRef } from "react";
import { UserType } from '../../types';
import Swal from "sweetalert2";
import Spinner from "../Spinner";
import { ToastContainer, toast } from "react-toastify";
import SearchBar from "../SearchBar";
import { useCreateUserMutation, useDeleteUserMutation, useGetAllUsersQuery, useUpdateUserMutation } from "../../store/apiquery/usersApiSlice";

/**
 * @function UpdateVendor
 * @description Form to update a vendor
 * @param {UserType} vendor - The vendor to be updated
 * @returns {JSX.Element} - A form with two text inputs for the first and last name, and a text input for the email and password
 */
const UpdateVendor = ({ vendor }: { vendor: UserType }) => {
  // const { data : categories } = useGetAllCategoriesQuery('api/categories')

  const [updateData, setUpdateData] = useState(vendor);
  const [updateVendor, udpateResult] = useUpdateUserMutation();
  const imageTag = useRef<HTMLImageElement>(null);
  const vendorId = vendor?.id;

  const [formData, setFormData] = useState({
    firstName: updateData.firstName,
    lastName: updateData.lastName,
    email: updateData.email,
    password: updateData.password,
  });

  /**
   * Handles the change event for the form fields.
   * If the field is a checkbox, it updates the formData with the checked status.
   * If the field is not a checkbox, it updates the formData with the value of the field.
   * @param e - The change event.
   */
  const handleUpdateValue = (
    e: React.ChangeEvent<
      HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement
    >
  ) => {
    const target = e.target as HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement;

    if (target instanceof HTMLInputElement && target.type === 'checkbox') {
      setFormData({ ...formData, [target.name]: target.checked });
    } else {
      setFormData({ ...formData, [target.name]: target.value });
    }
  };

  /**
   * Handles the submission of the form. Calls the updateVendor mutation and
   * displays a toast notification based on the result.
   * @param e - The form submission event.
   * @returns {Promise<void>}
   */
  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    try {
      const result = await updateVendor({ vendorId, formData });

      if ("data" in result && result.data) {
        console.log("vendor Updated successfully");
        toast.success("vendor Updated successfully");
        setFormData({
          firstName: "",
          lastName: "",
          email: "",
          password: "",
        });
      } else if ("error" in result && result.error) {
        console.error("vendor creation failed", result.error);
        toast.error("vendor creation failed");
      }
    } catch (error) {
      console.error("vendor creation failed`", error);
      toast.error("vendor creation failed");
    }
  };

  return (
    <form
      action=""
      method="patch"
      className="checkout-service p-3"
      onSubmit={handleSubmit}
    >
      <input type="hidden" name="id" value={updateData.id} />
      {/* <div className="w-25 mx-auto p-3 border border-1 rounded-5 fd-hover-border-primary" style={{ height: '250px' }}><img src={vendor.img} alt={vendor.name} className='w-100 h-100' ref={imageTag}/></div> */}
      <div className="d-flex gap-2">
        <label className="w-25">
          <span>First Name</span>
          <input
            type="text"
            name="firstName"
            value={formData.firstName}
            className="form-control w-100 rounded-0 p-2"
            onChange={handleUpdateValue}
          />
        </label>
        <label className="w-50">
          <span>Last Name</span>
          <input
            type="text"
            name="lastName"
            className="form-control w-100 rounded-0 p-2"
            value={formData.lastName}
            onChange={handleUpdateValue}
          />
        </label>
      </div>
      <div className="d-grid grid-4 gap-2 mt-3">
        <label>
          <span>Email</span>
          <input
            type="string"
            name="email"
            value={formData.email}
            className="form-control w-100 rounded-0 p-2"
            onChange={handleUpdateValue}
          />
        </label>
        <label>
          <span>Password</span>
          <input
            type="string"
            name="password"
            className="form-control w-100 rounded-0 p-2"
            value={formData.password}
            onChange={handleUpdateValue}
          />
        </label>
      </div>
      <div className="mt-4">
        <ToastContainer />
      </div>
      <div className="mt-3">
        {udpateResult.isLoading ? (
          <button className="fd-btn w-25 text-center border-0">
            <span
              className="spinner-grow spinner-grow-sm"
              role="status"
              aria-hidden="true"
            ></span>
            Loading...
          </button>
        ) : (
          <button className="fd-btn w-25 text-center border-0" type="submit">
            UPDATE vendor
          </button>
        )}
      </div>
    </form>
  );
};

/**
 * AddOrEditvendor component
 * 
 * This component renders a form for adding or editing a vendor.
 * If the vendor is null, it renders a form for adding a new vendor.
 * If the vendor is not null, it renders a form for editing the vendor.
 * The form includes fields for the vendor name, description, price, category, image, and active status.
 * It also includes a button for uploading an image and a button for saving the vendor.
 * When the form is submitted, it calls the createvendor or updatevendor mutation.
 * If the mutation is successful, it shows a success toast message.
 * If the mutation fails, it shows an error toast message.
 * @param {null | UserType} vendor - The vendor to be edited. If null, the form is for adding a new vendor.
 * @returns {JSX.Element} - The form for adding or editing a vendor.
 */
const AddOrEditvendor = ({ vendor }: { vendor: null | UserType }) => {
  // const [data, setData] = useState({});
  const [createvendor, result] = useCreateUserMutation();

  const [formData, setFormData] = useState({
    firstName: "",
    lastName: "",
    email: "",
    password: "",
  });

  /**
   * Handles the change event for the form fields.
   * Updates the formData state with the changed value.
   * If the field is a checkbox, it updates the formData with the checked status.
   * If the field is not a checkbox, it updates the formData with the value of the field.
   * @param e - The change event.
   */
  const handleValue = (
    e: React.ChangeEvent<
      HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement
    >
  ) => {
    const target = e.target as HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement;
    const { name, value } = target;
    const checked = (target as HTMLInputElement).checked;

    if (target.type === 'checkbox') {
      setFormData({ ...formData, [name]: checked });
    } else {
      setFormData({ ...formData, [name]: value });
    }
  };

  /**
   * Handles the submission of the form. Calls the createvendor mutation and
   * displays a toast notification based on the result.
   * @param e - The form submission event.
   * @returns {Promise<void>}
   */
  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    // handleUpload(); 

    try {
      const result = await createvendor({ formData });

      if ("data" in result && result.data) {
        console.log("vendor created successfully");
        toast.success("vendor created successfully");
        setFormData({
          firstName: "",
          lastName: "",
          email: "",
          password: "",
        });
      } else if ("error" in result && result.error) {
        console.error("vendor creation failed", result.error);
        toast.error("vendor creation failed");
      }
    } catch (error) {
      console.error("vendor creation failed`", error);
      toast.error("vendor creation failed");
    }
  };

  if (!vendor) {
    return (
      <form
        action=""
        method="post"
        className="checkout-service p-3 .form-vendor"
        onSubmit={handleSubmit}
      >
        <div className="d-flex gap-2">
          <label className="w-25">
            <span>First Name</span>
            <input
              type="text"
              name="firstName"
              value={formData.firstName}
              className="form-control w-100 rounded-0 p-2"
              placeholder="First Name"
              onChange={handleValue}
            />
          </label>
          <label className="w-50">
            <span>Last Name</span>
            <input
              type="text"
              name="lastName"
              value={formData.lastName}
              className="form-control w-100 rounded-0 p-2"
              placeholder="Last Name"
              onChange={handleValue}
            />
          </label>
        </div>
        <div className="d-grid grid-4 gap-2 mt-3">
          <label>
            <span>Email</span>
            <input
              type="string"
              step={0.1}
              name="email"
              value={formData.email}
              className="form-control w-100 rounded-0 p-2"
              placeholder="Email"
              onChange={handleValue}
            />
          </label>
          <label>
            <span>Password</span>
            <input
              type="number"
              step={0.1}
              name="password"
              value={formData.password}
              className="form-control w-100 rounded-0 p-2"
              placeholder="password"
              onChange={handleValue}
            />
          </label>
        </div>
        <div className="mt-3">
          <ToastContainer />
        </div>
        <div className="mt-3">
          {result.isLoading ? (
            <button className="fd-btn w-25 text-center border-0">
              <span
                className="spinner-grow spinner-grow-sm"
                role="status"
                aria-hidden="true"
              ></span>
              Loading...
            </button>
          ) : (
            <button
              className="fd-btn w-25 text-center border-0"
              // onClick={handleUpload}
            >
              SAVE NOW
            </button>
          )}
        </div>
      </form>
    );
  }

  return <UpdateVendor vendor={vendor} />;
};

  /**
   * Renders a table of vendors with the following columns:
   * - Vendor ID
   * - First Name
   * - Last Name
   * - Email
   * - Action (View, Edit, Delete)
   *
   * The component also renders a search input field that allows users to search for vendors by name, email, etc.
   *
   * The component also renders a spinner if the vendors list is loading.
   *
   * @param {{ setVendor: Function, setPage: Function }} props - an object with two properties: setVendor and setPage.
   * @returns {React.ReactNode} - a React node that renders the vendors table or a spinner if the vendors list is loading.
   */
const ListOfVendors = ({
  setVendor,
  setPage,
}: {
  setVendor: Function;
  setPage: Function;
}) => {
  const {
    isLoading,
    data: vendorsList,
    isSuccess,
    isError,
  } = useGetAllUsersQuery("api/users");
  const [deleteUser, deletedResult] = useDeleteUserMutation();
  
  /**
   * Sets the current vendor to the given vendor and changes the page to 'add'.
   * @param {UserType} vendor - the vendor to set as the current vendor
   */
  const parsevendor = (vendor: UserType) => {
    setVendor(vendor);
    setPage("add");
  };

  /**
   * Deletes a vendor with the given id.
   * It shows a confirmation dialog with a warning icon and asks the user if they are sure to delete the vendor.
   * If the user confirms, it calls the deleteUser function with the given id.
   * @param {string} id - the id of the vendor to delete
   */
  const deleteItem = (id: string) => {
    Swal.fire({
      title: "Are you sure?",
      text: "Are you sure to delete this vendor ?",
      icon: "warning",
      showCancelButton: true,
      confirmButtonColor: "#3085d6",
      cancelButtonColor: "#d33",
      confirmButtonText: "Yes, delete it!",
    }).then((r) => {
      if (r.isConfirmed) {
        deleteUser(id);
      }
    });
  };

  // search bar coding 
  const [searchInput, setSearchInput] = useState<string>('');

  let data: React.ReactNode;

  // Filter vendors based on the search input
    const filteredvendors = vendorsList?.data.filter((vendor: UserType) =>{
      const vendorname = vendor.firstName?.toLowerCase();
      const search = searchInput.toLowerCase();   
  
      return (
        vendorname?.includes(search)
      );
    });

    data =
    isLoading || isError
      ? null
      : isSuccess
      ? filteredvendors.map((vendor: UserType) => {
          // ? sortvendors.map((vendor: vendor) => {

          return (
            <tr className="p-3" key={vendor.id}>
              <td scope="row w-25">{vendor.id}</td>
              {/* <td scope="row w-25"><img src={vendor.img} alt={vendor.name} style={{ width: '50px', height: '50px' }} /></td> */}
              <td className="fw-bold">{vendor.firstName}</td>
              <td className="fw-bold">{vendor.lastName}</td>
              <td>{vendor.email}</td>
              <td className="fw-bold d-flex gap-2 justify-content-center">
                <a
                  href="#"
                  className="p-2 rounded-2 fd-bg-primary"
                  onClick={(e) => parsevendor(vendor)}
                  title="View vendor"
                >
                  <i className="bi bi-eye"></i>
                </a>
                <a
                  href="#"
                  className="p-2 rounded-2 bg-secondary"
                  onClick={(e) => parsevendor(vendor)}
                  title="Edit"
                >
                  <i className="bi bi-pen"></i>
                </a>
                <a
                  href="#"
                  className="p-2 rounded-2 bg-danger"
                  title="Delete"
                  onClick={(e) => {
                    e.preventDefault();
                    // deleteItem(vendor.id);
                    deleteItem((vendor as any)['id']);
                  }}
                >
                  <i className="bi bi-trash"></i>
                </a>
              </td>
            </tr>
          );
        })
      : null;

  return !isLoading ? (
    <div>
      {/* Add a search input field */}
      <div className="mb-3">
      <input
        type="text"
        placeholder="Search Vendors"
        value={searchInput}
        onChange={(e) => setSearchInput(e.target.value)}
        className="form-control w-100 p-2"
      />
    </div>
    <div className="table-responsive">
      <table className="table table-default text-center table-bordered">
        <thead>
          <tr className="fd-bg-primary text-white">
            <th scope="col" className="p-3">
              VENDOR ID
            </th>
            <th scope="col" className="p-3">
              FIRST NAME
            </th>
            <th scope="col" className="p-3">
              LAST NAME
            </th>
            <th scope="col" className="p-3">
              EMAIL
            </th>
            <th scope="col" className="p-3">
              ACTION
            </th>
          </tr>
        </thead>
        <tbody>{data}</tbody>
      </table>
    </div>
    </div>
  ) : (
    <Spinner />
  );
};

/**
 * The main component for the Vendors page. This component renders a list of vendors
 * and provides the ability to add a new vendor or edit an existing one.
 *
 * The component state is managed by the `useState` hook, which is used to store the
 * current page and the vendor to be edited.
 *
 * The component renders a list of vendors if the current page is "list", or it renders
 * the form to add or edit a vendor if the current page is "add".
 *
 * The component also renders a button to switch between the list and add/edit pages.
 *
 * @returns {JSX.Element} The rendered component.
 */
const VendorsMain = () => {
  const [page, setPage] = useState("list");
  const [currentvendor, setCurrentvendor] = useState(null);

  /**
   * Changes the page to "add" and resets the current vendor to null.
   * This function is used to switch from the list page to the add/edit page.
   */
  const changeToList = () => {
    setPage("add");
    setCurrentvendor(null);
  };
  
  /**
   * Changes the page to "list".
   * This function is used to switch from the add/edit page to the list page.
   */
  const changeToAdd = () => {
    setPage("list");
  };

  useEffect(() => setPage("list"), []);

  return (
    <div className="text-black">
      <h4 className="fw-bold">Vendors</h4>
      <div className="add-vendor my-3 d-flex justify-content-end">
        {page === "list" ? (
          <a
            href="#"
            className="fd-btn bg-secondary w-25 text-center rounded-3"
            onClick={changeToList}
          >
            ADD VENDOR
          </a>
        ) : (
          <a
            href="#"
            className="fd-btn bg-secondary w-25 text-center rounded-3"
            onClick={changeToAdd}
          >
            VENDORS LIST
          </a>
        )}
      </div>
      <div className="subPartMain">
        {page === "list" ? (
          <ListOfVendors setVendor={setCurrentvendor} setPage={setPage} />
        ) : (
          <AddOrEditvendor vendor={currentvendor} />
        )}
      </div>
    </div>
  );
};

export default VendorsMain;
