const localStorageMock = {
  getItem: jest.fn(),
  setItem: jest.fn(),
  removeItem: jest.fn(),
  clear: jest.fn(),
};
global.localStorage = localStorageMock;

// Mock the request issued by the react app to get the client configuration parameters.
window.fetch = () => {
  return Promise.resolve(
    {
      ok: true,
      json: () => Promise.resolve({
        "authority": "https://localhost:7269",
        "client_id": "SolarFix",
        "redirect_uri": "https://localhost:7269/authentication/login-callback",
        "post_logout_redirect_uri": "https://localhost:7269/authentication/logout-callback",
        "response_type": "id_token token",
        "scope": "SolarFixAPI openid profile"
     })
    });
};
