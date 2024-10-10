import { useEffect } from "react";
import { ToastContainer, toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import { setItem } from "../Utils/Generals";
import { useNavigate } from "react-router-dom";
import RoutePaths from "../config";
import { useAppDispatch } from "../hooks/redux-hooks";
import { setUser } from "../store/userSlice";

/**
 * Handles the result of an API request and displays a toast message accordingly.
 *
 * @param {{ result: any }} props - An object with a single property, `result`, which
 *     contains the result of an API request.
 */
export const HandleResult = ({ result }: { result: any }) => {
    const navigate = useNavigate();
    const dispatch = useAppDispatch();

  useEffect(() => {
    if (result.isError) {
      // Handle error response
      toast.error(result.error.message || "An error occurred.");
    } else if (result.isSuccess) {
      // Handle success response
      const responseData = result.data;

      if (responseData.accessToken) {
        // Assuming responseData.tokens.accessToken contains the access token
        setItem(RoutePaths.token, responseData.accessToken);

        // Assuming responseData.user contains the user data
        const userRole = responseData.role;
        setItem("user", responseData);
        setItem("userRole", userRole); 
        dispatch(setUser(responseData));

        // Redirect to the appropriate route based on user data
        navigate(
          ["Admin", "Vendor", "Csr"].some(role => responseData.role.includes(role))
            ? RoutePaths.admin
            : RoutePaths.login
        );
      } else {
        // Handle invalid response data
        toast.error("Invalid response data.");
      }

      toast.success(responseData.message || "Login successful.");
    }
  }, [result]);

  return (
    <>
      <ToastContainer />
    </>
  );
};
