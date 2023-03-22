import React from "react";
import { Route, Redirect } from "react-router-dom";
import { ConnectedRouter } from "react-router-redux";
// import { ConnectedRouter } from "connected-react-router";
import { connect } from "react-redux";
import App from "./containers/App/App";
import asyncComponent from "./helpers/AsyncFunc";
import Auth0 from "./helpers/auth0";
import { isFullLocalStorage } from "./helpers/utility";
const localStorageNotNull = isFullLocalStorage();

const RestrictedRoute = ({ component: Component, isLoggedIn, ...rest }) => (
  <Route
    {...rest}
    render={(props) =>
      isLoggedIn ? (
        <Component {...props} />
      ) : (
        <Redirect
          to={{
            pathname: "/signin",
            state: { from: props.location },
          }}
        />
      )
    }
  />
);
const PublicRoutes = ({ history, isLoggedIn }) => {
  return (
    <ConnectedRouter history={history}>
      <div>
        <Route exact path={"/"} component={asyncComponent(() => import("./containers/Page/signin"))} />
        <Route exact path={"/404"} component={asyncComponent(() => import("./containers/Page/404"))} />
        <Route exact path={"/500"} component={asyncComponent(() => import("./containers/Page/500"))} />
        <Route exact path={"/signin"} component={asyncComponent(() => import("./containers/Page/signin"))} />
        <Route exact path={"/signup"} component={asyncComponent(() => import("./containers/Page/signup"))} />
        <Route exact={false} path={"/auth/sso"} component={asyncComponent(() => import("./customApp/containers/DangNhapSSO/index"))} />
        <Route exact path={"/forgotpassword"} component={asyncComponent(() => import("./containers/Page/forgotPassword"))} />
        <Route exact path={"/resetpassword"} component={asyncComponent(() => import("./containers/Page/resetPassword"))} />

        <Route
          path="/auth0loginCallback"
          render={(props) => {
            Auth0.handleAuthentication(props);
          }}
        />
        <RestrictedRoute path="/dashboard" component={App} isLoggedIn={isLoggedIn} />
      </div>
    </ConnectedRouter>
  );
};

export default connect((state) => ({
  isLoggedIn: state.Auth.idToken !== null || localStorageNotNull,
  //da dang nhap khi co reduce idToken hoac co localStore
}))(PublicRoutes);
