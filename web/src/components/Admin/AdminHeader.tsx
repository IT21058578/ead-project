import React from "react";
import { Link } from "react-router-dom";
import RoutePaths from "../../config";
import logo from "../../assets/img/logo.png";
import Lang from "../Lang";
import { SocialsNetworks } from "../SocialsNetworks";
import NotificationsList from "../NotificationsList";

const AdminHeader = () => {
  return (
    <div>
      <div className="d-lg-flex justify-content-between font-light fd-bg-secondary px-3 py-2 px-lg-5">
        <div className="d-flex header-contacts d-none d-lg-block gap-3">
          <span>
            <i className="bi bi-envelope fd-color-primary"></i>&nbsp;
            StyleSquare@gmail.com
          </span>
          <span>
            <i className="bi bi-geo-alt fd-color-primary"></i>&nbsp; Colombo,
            Sri Lanka
          </span>
        </div>
        <div className="d-flex justify-content-between header-socials-lang">
          <SocialsNetworks />
          <Lang />
        </div>
      </div>
      <div className="header bg-white text-black shadow d-flex justify-content-between px-5 py-2">
        <div className="img align-self-center">
          <Link to={"/admin"}>
            <img src={logo} alt="" style={{ height: "50px" }} />
          </Link>
        </div>
        <div className="welcome-msg align-self-center">
          <h5 className="fw-bold opacity-75">Store Management Dashboard</h5>
        </div>
        <div className="navigation font-regular d-flex flex-wrap justify-content-between py-4">
          <div className="d-flex gap-2 align-self-center">
            <div>
              {/* <a
                href="#"
                className="position-relative border-3 shadow border-light py-2 px-3 text-dark fd-hover-bg-primary"
              >

                <NotificationsList />
              </a> */}
              <NotificationsList />
            </div>
            {/* <div>
            <Link
              to="#"
              className="position-relative border-3 shadow border-light py-2 px-3 text-dark fd-hover-bg-primary"
            >
              <i className="bi bi-messenger"></i>
              <span className="position-absolute top-0">3</span>
            </Link>
          </div> */}
            <div>
              <Link
                to={RoutePaths.adminAccount}
                className="position-relative border-3 shadow border-light py-2 px-3 text-dark fd-hover-bg-primary"
              >
                <i className="bi bi-person"></i>
              </Link>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default AdminHeader;
