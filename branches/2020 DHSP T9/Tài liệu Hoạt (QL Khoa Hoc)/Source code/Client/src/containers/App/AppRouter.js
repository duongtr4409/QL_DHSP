import React, { Component } from "react";
import { BrowserRouter, Route, Switch } from "react-router-dom";
import asyncComponent from "../../helpers/AsyncFunc";
import customRoutes from "../../customApp/router";
import { store } from "../../redux/store";

const routes = [...customRoutes];

class AppRouter extends Component {
  render() {
    const { url, style } = this.props;
    //get list routes ---------########
    let role = store.getState().Auth.role;
    if (!role) {
      let roleStore = localStorage.getItem("role");
      role = JSON.parse(roleStore);
    }
    let listRoutes = [];
    routes.forEach((value) => {
      if (!value.path || (role && role[value.path] && role[value.path].view) || value.isDetail) {
        //################Phan quyen menu-router
        listRoutes.push(value);
      }
    });
    return (
      <div style={style}>
        {listRoutes.map((singleRoute) => {
          const { path, exact, ...otherProps } = singleRoute;
          return <Route exact={!(exact === false)} key={singleRoute.path} path={`${url}/${singleRoute.path}`} {...otherProps} />;
        })}
      </div>
    );
  }
}

export default AppRouter;
