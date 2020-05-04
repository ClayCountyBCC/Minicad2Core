import React, { useContext, useEffect } from 'react';
import { Store } from './Store';

const Nav = () =>
{
  const { state, dispatch } = useContext(Store);

  const closeBaseMapDropdown = () =>
  {
    let e = document.getElementById("nav_basemap_dropdown");
    if (e === null) return;
    e.classList.remove("is-active");
  }

  return (
    <nav className="navbar is-light" role="navigation" aria-label="main navigation">
      <div className="navbar-brand">
        <h1 className="navbar-item is-size-3 has-text-weight-bold">MINICAD</h1>
      </div>
      <div className="navbar-menu">
        <div className="navbar-start">
          <p            
            onClick={event =>
            {
              dispatch({ type: "set_current_view", payload: "active calls" });
            }}
            className={`navbar-item is-tab cursor_pointer ${state.current_view === "active calls" ? 'is-active' : ''} `}>
            Active Calls
          </p>
          <p
            onClick={event =>
            {
              dispatch({ type: "set_current_view", payload: "unit status" });
            }}
            className={`navbar-item is-tab cursor_pointer ${state.current_view === "unit status" ? 'is-active' : ''} `}>
            Unit Status
          </p>
          <p
            onClick={event =>
            {
              dispatch({ type: "set_current_view", payload: "advisories" });
            }}
            className={`navbar-item is-tab cursor_pointer ${state.current_view === "advisories" ? 'is-active' : ''} `}>
            Advisories
          </p>
          <p
            onClick={event =>
            {
              dispatch({ type: "set_current_view", payload: "radio" });
            }}
            className={`navbar-item is-tab cursor_pointer ${state.current_view === "radio" ? 'is-active' : ''} `}>
            Radio
          </p>
          <p
            onClick={event =>
            {
              dispatch({ type: "set_current_view", payload: "map" });
            }}
            className={`navbar-item is-tab cursor_pointer ${state.current_view === "map" ? 'is-active' : ''} `}>
            Map            
          </p>
        </div>

        <div
          style={{paddingRight: "4em"}}
          className="navbar-end">
          <div
            id="nav_basemap_dropdown"
            className="navbar-item has-dropdown">
            <a className="navbar-link"
              onClick={event =>
              {
                let e = document.getElementById("nav_basemap_dropdown");
                if (e === null) return;
                let isActive = e.classList.contains("is-active");
                let basemaps = document.querySelectorAll(".esri-basemap-gallery__item");
                basemaps.forEach(b =>
                {
                  if (!isActive)
                  {
                    b.addEventListener("click", closeBaseMapDropdown);
                  }
                  else
                  {
                    b.removeEventListener("click", closeBaseMapDropdown);
                  }
                });
                e.classList.toggle("is-active");
              }}
            >
              Select Base Map
            </a>
            <div id="basemap-navbar-dropdown"
              className="navbar-dropdown">
              <div
                className="navbar-item">
                <div id="basemap_selector_container"></div>

              </div>
            </div>
          </div>
          <div id="nav_layerlist_dropdown"
            className="navbar-item has-dropdown">
            <a className="navbar-link"
              onClick={event =>
              {
                let e = document.getElementById("nav_layerlist_dropdown");
                if (e === null) return;
                e.classList.toggle("is-active");
              }}
            >
              View Layers
            </a>
            <div className="navbar-dropdown">
              <div
                className="navbar-item">
                <div id="layerlist_selector_container"></div>

              </div>
            </div>

          </div>
          <div className="navbar-item">
            <span className="icon is-medium">
                <i className="fas fa-user fa-2x"></i>
            </span>
          </div>
        </div>
      </div>
    </nav>
  );
}

export default Nav;
