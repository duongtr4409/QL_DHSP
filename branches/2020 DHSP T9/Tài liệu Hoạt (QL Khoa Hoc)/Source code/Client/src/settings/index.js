const date = new Date();
const currentYear = date.getFullYear();
export default {
  apiUrl: "https://api.2bsystem.com.vn/api/v1/", //Test
  // apiUrl: "http://192.168.100.46:7002/api/v1/", //Dev
  // apiUrl: 'https://localhost:44320/api/v1/', //Local
};
const siteConfig = {
  siteName: "",
  siteIcon: "", //ion-flash
  footerText: ``,
};

const themeConfig = {
  topbar: "theme8",
  sidebar: "theme8",
  layout: "theme2",
  theme: "themedefault",
};
const language = "english";

const AlgoliaSearchConfig = {
  appId: "",
  apiKey: "",
};
const Auth0Config = {
  domain: "",
  clientID: "",
  allowedConnections: ["Username-Password-Authentication"],
  rememberLastLogin: true,
  language: "en",
  closable: true,
  options: {
    auth: {
      autoParseHash: true,
      redirect: true,
      redirectUrl: "http://localhost:3000/auth0loginCallback",
    },
    languageDictionary: {
      title: "Isomorphic",
      emailInputPlaceholder: "demo@gmail.com",
      passwordInputPlaceholder: "demodemo",
    },
    theme: {
      labeledSubmitButton: true,
      logo: "",
      primaryColor: "#E14615",
      authButtons: {
        connectionName: {
          displayName: "Log In",
          primaryColor: "#b7b7b7",
          foregroundColor: "#000000",
        },
      },
    },
  },
};
const firebaseConfig = {
  apiKey: "",
  authDomain: "",
  databaseURL: "",
  projectId: "",
  storageBucket: "",
  messagingSenderId: "",
};
const googleConfig = {
  apiKey: "", //
};
const mapboxConfig = {
  tileLayer: "",
  maxZoom: "",
  defaultZoom: "",
  center: [],
};
const youtubeSearchApi = "";
export { siteConfig, themeConfig, language, AlgoliaSearchConfig, Auth0Config, firebaseConfig, googleConfig, mapboxConfig, youtubeSearchApi };
