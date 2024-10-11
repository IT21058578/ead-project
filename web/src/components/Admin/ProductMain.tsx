import React, { useState, useEffect, useRef } from "react";
import { Product } from '../../types';
import {
  useCreateProductMutation,
  useDeleteProductMutation,
  useGetAllProductsQuery,
  useGetVendorProductsQuery,
  useUpdateProductMutation,
} from "../../store/apiquery/productApiSlice";
import Swal from "sweetalert2";
import Spinner from "../Spinner";
import { useUploadImagesMutation } from "../../store/apiquery/productApiSlice";
import { ToastContainer, toast } from "react-toastify";
import SearchBar from "../SearchBar";
import { getItem } from "../../Utils/Generals";

let imageIsChanged = false;

/**
 * @function UpdateProduct
 * @description Form to update a product
 * @param {Product} product - The product to be updated
 * @returns {JSX.Element} - A form with all the product details
 */
const UpdateProduct = ({ product }: { product: Product }) => {
  // const { data : categories } = useGetAllCategoriesQuery('api/categories')
  const userRole = getItem('userRole');

  const [updateData, setUpdateData] = useState(product);
  const [updateProduct, udpateResult] = useUpdateProductMutation();
  const imageTag = useRef<HTMLImageElement>(null);
  const productId = product?.id;

  const [formData, setFormData] = useState({
    vendorId: updateData.vendorId,
    name: updateData.name,
    description: updateData.description,
    category: updateData.category,
    price: updateData.price,
    isActive: updateData.isActive,
    countInStock: updateData.countInStock,
    lowStockThreshold: updateData.lowStockThreshold,
    imageUrl: updateData.imageUrl,
  });

  /**
   * @function handleUpdateValue
   * @description Handle the change event on any of the form elements
   * @param {React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement>} e - The event
   * @returns {void}
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
   * @function handleSubmit
   * @description Handle the submission of the form. Calls the updateProduct mutation and
   * displays a toast notification based on the result.
   * @param {React.FormEvent} e - The event
   * @returns {Promise<void>}
   */
  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    try {
      const result = await updateProduct({ productId, formData });

      if ("data" in result && result.data) {
        console.log("Product Updated successfully");
        toast.success("Product Updated successfully");
        // setFormData({
        //   vendorId: "",
        //   name: "",
        //   description: "",
        //   category: "",
        //   price: 0,
        //   isActive: false,
        //   countInStock: 0,
        //   lowStockThreshold: 0,
        //   imageUrl: "",
        // });
      } else if ("error" in result && result.error) {
        console.error("Product creation failed", result.error);
        toast.error("Product creation failed");
      }
    } catch (error) {
      console.error("Product creation failed`", error);
      toast.error("Product creation failed");
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
      <div
        className="w-25 mx-auto p-3 border border-1 rounded-5 fd-hover-border-primary"
        style={{ height: "250px" }}
      >
        <img
          src={product.imageUrl}
          alt={product.name}
          className="w-100 h-100"
          ref={imageTag}
        />
      </div>
      {/* <div className="w-25 mx-auto p-3 border border-1 rounded-5 fd-hover-border-primary" style={{ height: '250px' }}><img src={product.img} alt={product.name} className='w-100 h-100' ref={imageTag}/></div> */}
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
            value={formData.name}
            onChange={handleUpdateValue}
          />
        </label>
      </div>
      <div className="d-grid grid-4 gap-2 mt-3">
        <label>
          <span>Category</span>
          <input
            type="string"
            step={0.1}
            name="category"
            value={formData.category}
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
            value={formData.price}
            onChange={handleUpdateValue}
          />
        </label>
        <label>
          <span>In Stock Quantity</span>
          <input
            type="number"
            name="countInStock"
            value={formData.countInStock}
            className="form-control w-100 rounded-0 p-2"
            onChange={handleUpdateValue}
            disabled={userRole !== 'Admin'}
          />
        </label>
        <label>
          <span>Low Stock Threshold</span>
          <input
            type="text"
            name="lowStockThreshold"
            value={formData.lowStockThreshold}
            className="form-control w-100 rounded-0 p-2"
            onChange={handleUpdateValue}
            disabled={userRole !== 'Admin'}
          />
        </label>
        <label className="form-check form-switch pt-4 pl-3">
          <span className="form-check-label">Active</span>
          <input
            type="checkbox"
            name="isActive"
            checked={formData.isActive ? true : false }
            className="form-check-input"
            onChange={handleUpdateValue}
            disabled={userRole !== 'Admin'}
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
            onChange={handleUpdateValue}
          ></textarea>
        </label>
        <label className="w-50">
          <span>Image URL</span>
          <input
            type="text"
            name="imageUrl"
            value={formData.imageUrl}
            className="form-control w-100 rounded-0 p-2"
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
            UPDATE PRODUCT
          </button>
        )}
      </div>
    </form>
  );
};

/**
 * This component renders a form for adding or editing a product.
 * It takes a product property which is an object with the product details.
 * If the product is null, it renders a form for adding a new product.
 * If the product is not null, it renders a form for editing the product.
 * The form includes fields for the product name, description, price, category, image, and active status.
 * It also includes a button for uploading an image and a button for saving the product.
 * When the form is submitted, it calls the createProduct or updateProduct mutation.
 * If the mutation is successful, it shows a success toast message.
 * If the mutation fails, it shows an error toast message.
 * @param {null | Product} product - The product to be edited. If null, the form is for adding a new product.
 * @returns {JSX.Element} - The form for adding or editing a product.
 */
const AddOrEditProduct = ({ product }: { product: null | Product }) => {
  // const [data, setData] = useState({});

  // const { data : categories } = useGetAllCategoriesQuery('api/categories')
  const userRole = getItem('userRole');

  const [createProduct, result] = useCreateProductMutation();
  const [image, setImage] = useState<File | null>(null);

  const [file, setFile] = React.useState(null);
  const [uploadFile, { isLoading }] = useUploadImagesMutation();

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

  /**
   * Handles the change event for the form fields.
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
   * Handles the form submission event.
   * Calls the createProduct mutation and sets the form data to the initial state if successful.
   * If the mutation fails, it logs the error and displays an error toast.
   * @param e - The form submission event.
   */
  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    // handleUpload(); 

    try {
      const result = await createProduct({ formData });

      if ("data" in result && result.data) {
        console.log("Product created successfully");
        toast.success("Product created successfully");
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
        console.error("Product creation failed", result.error);
        toast.error("Product creation failed");
      }
    } catch (error) {
      console.error("Product creation failed`", error);
      toast.error("Product creation failed");
    }
  };

  if (!product) {
    return (
      <form
        action=""
        method="post"
        className="checkout-service p-3 .form-product"
        onSubmit={handleSubmit}
      >
        {image && (
          <div
            className="w-25 mx-auto p-3 border border-1 rounded-5 fd-hover-border-primary mb-4"
            style={{ height: "250px" }}
          >
            <img
              src={URL.createObjectURL(image)}
              alt="Product Image Preview"
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
              placeholder="Product Name"
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
              placeholder="Product Price"
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
              disabled={userRole !== 'Admin'}
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
              disabled={userRole !== 'Admin'}
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
              disabled={userRole !== 'Admin'}
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

  return <UpdateProduct product={product} />;
};

/**
 * ListOfProducts component
 * 
 * This component renders a table of products with the following columns:
 * - Image
 * - Product Name
 * - Price
 * - Total Stock
 * - Active
 * - Action (View, Edit, Delete)
 * 
 * The component also renders a search input field that allows users to search for products by name, price, or total stock.
 * 
 * The component also renders a spinner if the products list is loading.
 * 
 * @param {{ setProduct: Function, setPage: Function }} props - an object with two properties: setProduct and setPage.
 * @returns {React.ReactNode} - a React node that renders the products table or a spinner if the products list is loading.
 */
const ListOfProducts = ({
  setProduct,
  setPage,
}: {
  setProduct: Function;
  setPage: Function;
}) => {
  const userRole = getItem('userRole');

  const {
    isLoading,
    data: productsList,
    isSuccess,
    isError,
  } = userRole === 'Vendor'
    ? useGetVendorProductsQuery("api/products")
    : useGetAllProductsQuery("api/products");
  const [deleteProduct, deletedResult] = useDeleteProductMutation();
  /**
   * parseProduct function
   * 
   * This function takes a Product object as an argument and sets the product state to the given product.
   * It also sets the page state to "add".
   * 
   * @param {Product} product - a Product object
   */
  const parseProduct = (product: Product) => {
    setProduct(product);
    setPage("add");
  };

  /**
   * deleteItem function
   * 
   * This function takes a string id as an argument and deletes the product with the given id.
   * It shows a confirmation dialog with a warning icon and asks the user if they are sure to delete the product.
   * If the user confirms, it calls the deleteProduct function with the given id.
   * 
   * @param {string} id - the id of the product to delete
   */
  const deleteItem = (id: string) => {
    Swal.fire({
      title: "Are you sure?",
      text: "Are you sure to delete this product ?",
      icon: "warning",
      showCancelButton: true,
      confirmButtonColor: "#3085d6",
      cancelButtonColor: "#d33",
      confirmButtonText: "Yes, delete it!",
    }).then((r) => {
      if (r.isConfirmed) {
        deleteProduct(id);
      }
    });
  };

  // search bar coding 
  const [searchInput, setSearchInput] = useState<string>('');

  let data: React.ReactNode;

  // Filter products based on the search input
    const filteredProducts = productsList?.data.filter((product: Product) =>{
      const productname = product.name?.toLowerCase();
      const search = searchInput.toLowerCase();
    
      // Convert numbers to strings before searching
      const price = product.price?.toString();
      const totalstock = product.countInStock?.toString();
  
      return (
        productname?.includes(search) ||
        price?.includes(search) ||
        totalstock?.includes(search)
      );
    });

    data =
    isLoading || isError
      ? null
      : isSuccess
      ? filteredProducts.map((product: Product) => {
          // ? sortProducts.map((product: Product) => {

          return (
            <tr className="p-3" key={product.id}>
              <td scope="row w-25">
                <img
                  src={product.imageUrl}
                  alt={product.name}
                  style={{ width: "50px", height: "50px" }}
                />
              </td>
              {/* <td scope="row w-25"><img src={product.img} alt={product.name} style={{ width: '50px', height: '50px' }} /></td> */}
              <td className="fw-bold">{product.name}</td>
              <td>{product.price}</td>
              <td>{product.countInStock}</td>
              <td>
                {product.isActive ? (
                  <i className="bi bi-check-circle-fill" style={{ color: "green" }}></i>
                ) : (
                  <i className="bi bi-x-circle-fill" style={{ color: "red" }}></i>
                )}
              </td>
              <td className="fw-bold d-flex gap-2 justify-content-center">
                <a
                  href="#"
                  className="p-2 rounded-2 fd-bg-primary"
                  onClick={(e) => parseProduct(product)}
                  title="View Product"
                >
                  <i className="bi bi-eye"></i>
                </a>
                <a
                  href="#"
                  className="p-2 rounded-2 bg-secondary"
                  onClick={(e) => parseProduct(product)}
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
                    // deleteItem(product.id);
                    deleteItem((product as any)['id']);
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
        placeholder="Search Products"
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
              IMAGE
            </th>
            <th scope="col" className="p-3">
              PRODUCT NAME
            </th>
            <th scope="col" className="p-3">
              PRICE
            </th>
            <th scope="col" className="p-3">
              TOTAL STOCK
            </th>
            <th scope="col" className="p-3">
              ACTIVE
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
 * The main component for the Products page. This component renders a list of products
 * and provides the ability to add a new product or edit an existing one.
 *
 * The component state is managed by the `useState` hook, which is used to store the
 * current page and the product to be edited.
 *
 * The component renders a list of products if the current page is "list", or it renders
 * the form to add or edit a product if the current page is "add".
 *
 * The component also renders a button to switch between the list and add/edit pages.
 *
 * @returns {JSX.Element} The rendered component.
 */
const ProductMain = () => {
  const [page, setPage] = useState("list");
  const [currentProduct, setCurrentProduct] = useState(null);

  /**
   * Change the page to "add" and reset the current product to null.
   * This function is used to switch from the list page to the add/edit page.
   */
  const changeToList = () => {
    setPage("add");
    setCurrentProduct(null);
  };
  /**
   * Change the page to "list".
   * This function is used to switch from the add/edit page to the list page.
   */
  const changeToAdd = () => {
    setPage("list");
  };

  useEffect(() => setPage("list"), []);

  return (
    <div className="text-black">
      <h4 className="fw-bold">Products</h4>
      <div className="add-product my-3 d-flex justify-content-end">
        {page === "list" ? (
          <a
            href="#"
            className="fd-btn bg-secondary w-25 text-center rounded-3"
            onClick={changeToList}
          >
            ADD PRODUCT
          </a>
        ) : (
          <a
            href="#"
            className="fd-btn bg-secondary w-25 text-center rounded-3"
            onClick={changeToAdd}
          >
            PRODUCTS LIST
          </a>
        )}
      </div>
      <div className="subPartMain">
        {page === "list" ? (
          <ListOfProducts setProduct={setCurrentProduct} setPage={setPage} />
        ) : (
          <AddOrEditProduct product={currentProduct} />
        )}
      </div>
    </div>
  );
};

export default ProductMain;
