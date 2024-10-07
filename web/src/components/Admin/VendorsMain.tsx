import React, { useState, useEffect, useRef } from "react";
import { UserType } from '../../types';
import Swal from "sweetalert2";
import Spinner from "../Spinner";
import { ToastContainer, toast } from "react-toastify";
import { Category } from "../../views/includes/Section";
import SearchBar from "../SearchBar";
import { useCreateUserMutation, useDeleteUserMutation, useGetAllUsersQuery, useUpdateUserMutation } from "../../store/apiquery/usersApiSlice";

let imageIsChanged = false;

const UpdateVendor = ({ vendor }: { vendor: UserType }) => {
  // const { data : categories } = useGetAllCategoriesQuery('api/categories')

  const [updateData, setUpdateData] = useState(vendor);
  const [updateVendor, udpateResult] = useUpdateUserMutation();
  const imageTag = useRef<HTMLImageElement>(null);
  const vendorId = vendor?.id;

  const [formData, setFormData] = useState({
    vendorId: updateData.id,
    firstName: updateData.firstName,
    lastName: updateData.lastName,
    email: updateData.email,
  });

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

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    try {
      const result = await updateVendor({ vendorId, formData });

      if ("data" in result && result.data) {
        console.log("vendor Updated successfully");
        toast.success("vendor Updated successfully");
        setFormData({
          vendorId: "",
          firstName: "",
          lastName: "",
          email: "",
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
          <span>Vendor ID</span>
          <input
            type="text"
            name="vendorId"
            value={formData.vendorId}
            className="form-control w-100 rounded-0 p-2"
            placeholder="Vendor ID"
            onChange={handleUpdateValue}
          />
        </label>
        <label className="w-50">
          <span>Name</span>
          <input
            type="text"
            name="name"
            className="form-control w-100 rounded-0 p-2"
            value={formData.firstName}
            onChange={handleUpdateValue}
          />
        </label>
        {/* <label className="w-50">
          <span>Image</span>
          <input
            type="file"
            name="images"
            value={formData.images}
            className="form-control w-100 rounded-0 p-2"
            placeholder="Change Image"
            onChange={handleUpdateValue}
          />
        </label> */}
      </div>
      <div className="d-grid grid-4 gap-2 mt-3">
        <label>
          <span>Category</span>
          <input
            type="string"
            step={0.1}
            name="category"
            value={formData.lastName}
            className="form-control w-100 rounded-0 p-2"
            onChange={handleUpdateValue}
          />
        </label>
        <label>
          <span>Price - LKR</span>
          <input
            type="number"
            name="price"
            className="form-control w-100 rounded-0 p-2"
            value={formData.email}
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

const AddOrEditvendor = ({ vendor }: { vendor: null | UserType }) => {
  // const [data, setData] = useState({});

  // const { data : categories } = useGetAllCategoriesQuery('api/categories')

  const [createvendor, result] = useCreateUserMutation();
  // const [uploadImages] = useUploadImagesMutation(); // Destructure the mutation function
  const [image, setImage] = useState<File | null>(null);

  // const handleImageChange = (e: React.ChangeEvent<HTMLInputElement>) => {
  //   if (e.target.files && e.target.files[0]) {
  //     setImage(e.target.files[0]);
  //   }
  // };

  // const handleImageUpload = async () => {
  //   if (image) {
  //     const formData = new FormData();
  //     formData.append("file", image);

  //     try {
  //       const result = await uploadImages(formData);
  //       if ("data" in result && result.data) {
  //         console.log("Image uploaded successfully");
  //       } else if ("error" in result && result.error) {
  //         console.error("Image upload failed", result.error);
  //       }
  //     } catch (error) {
  //       console.error("Image upload failed", error);
  //     }
  //   }
  // };

  const [file, setFile] = React.useState(null);

  const handleFileChange = (e:any) => {
    setFile(e.target.files[0]);
  };

  const [formData, setFormData] = useState({
    vendorId: "",
    name: "",
    description: "",
    category: "",
    price: 0,
    isActive: false,
    countInStock: 0,
    lowStockThreshold: 0,
    imageUrl: "",
  });

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

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    // handleUpload(); 

    try {
      const result = await createvendor({ formData });

      if ("data" in result && result.data) {
        console.log("vendor created successfully");
        toast.success("vendor created successfully");
        setFormData({
          vendorId: "",
          name: "",
          description: "",
          category: "",
          price: 0,
          isActive: false,
          countInStock: 0,
          lowStockThreshold: 0,
          imageUrl: "",
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
        {image && (
          <div
            className="w-25 mx-auto p-3 border border-1 rounded-5 fd-hover-border-primary mb-4"
            style={{ height: "250px" }}
          >
            <img
              src={URL.createObjectURL(image)}
              alt="vendor Image Preview"
              className="w-100 h-100"
            />
          </div>
        )}
        <div className="d-flex gap-2">
          <label className="w-25">
            <span>Vendor ID</span>
            <input
              type="text"
              name="vendorId"
              value={formData.vendorId}
              className="form-control w-100 rounded-0 p-2"
              placeholder="Vendor ID"
              onChange={handleValue}
            />
          </label>
          <label className="w-50">
            <span>Name</span>
            <input
              type="text"
              name="name"
              value={formData.name}
              className="form-control w-100 rounded-0 p-2"
              placeholder="vendor Name"
              onChange={handleValue}
            />
          </label>
          <label className="w-25">
            <span>Category</span>
            <input
              type="string"
              step={0.1}
              name="category"
              value={formData.category}
              className="form-control w-100 rounded-0 p-2"
              placeholder="Category"
              onChange={handleValue}
            />
          </label>
          {/* <label className="w-50">
            <span>Image</span>
            <input
              type="file"
              name="images"
              // value={formData.images}
              className="form-control w-100 rounded-0 p-2"
              placeholder="vendor Image"
              onChange={handleFileChange}
              accept="image/*"
            />
          </label> */}
        </div>
        <div className="d-grid grid-4 gap-2 mt-3">
          <label>
            <span>Price - LKR</span>
            <input
              type="number"
              step={0.1}
              name="price"
              value={formData.price}
              className="form-control w-100 rounded-0 p-2"
              placeholder="vendor Price"
              onChange={handleValue}
            />
          </label>
          <label>
            <span>In Stock Quantity</span>
            <input
              type="number"
              name="countInStock"
              value={formData.countInStock}
              className="form-control w-100 rounded-0 p-2"
              placeholder="Quantity"
              onChange={handleValue}
            />
          </label>
          <label>
            <span>Low Stock Threshold</span>
            <input
              type="text"
              name="lowStockThreshold"
              value={formData.lowStockThreshold}
              className="form-control w-100 rounded-0 p-2"
              placeholder="Low Stock Threshold"
              onChange={handleValue}
            />
          </label>
          <label className="form-check form-switch pt-4 pl-3">
            <span className="form-check-label">Active</span>
            <input
              type="checkbox"
              name="isActive"
              checked={formData.isActive ? true : false }
              className="form-check-input"
              onChange={handleValue}
            />
          </label>
        </div>
        <div className="d-flex gap-2">
          <label className="w-50">
            <span>Description</span>
            <textarea
              name="description"
              cols={100}
              rows={5}
              value={formData.description}
              className="w-100 p-2 border"
              placeholder="Description"
              onChange={handleValue}
            ></textarea>
          </label>
          <label className="w-50">
            <span>Image URL</span>
            <input
              type="text"
              name="imageUrl"
              value={formData.imageUrl}
              className="form-control w-100 rounded-0 p-2"
              placeholder="Image URL"
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
  const parsevendor = (vendor: UserType) => {
    setVendor(vendor);
    setPage("add");
  };

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
              VENDOR FIRST NAME
            </th>
            <th scope="col" className="p-3">
              VENDOR LAST NAME
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

const VendorsMain = () => {
  const [page, setPage] = useState("list");
  const [currentvendor, setCurrentvendor] = useState(null);

  const changeToList = () => {
    setPage("add");
    setCurrentvendor(null);
  };
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
