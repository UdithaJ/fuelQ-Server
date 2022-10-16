package com.example.client;

import android.os.Bundle;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.AutoCompleteTextView;
import android.widget.Button;
import android.widget.EditText;

import androidx.annotation.NonNull;
import androidx.fragment.app.Fragment;
import androidx.navigation.fragment.NavHostFragment;
import com.example.client.databinding.DriverSignUpPageBinding;
import com.google.android.material.textfield.TextInputLayout;
import com.google.gson.Gson;

import org.json.JSONException;
import org.json.JSONObject;

import java.util.List;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class DriverSignUpActivity extends Fragment  {

private DriverSignUpPageBinding binding;

String[] fuelTypes = {"Petrol", "Diesel"};
String[] vehicleTypes = {"Bike", "Car/Van", "ThreeWheels", "Bus"};
AutoCompleteTextView fuelType;
AutoCompleteTextView vehicleType;
ArrayAdapter<String> fuelTypeAdapterItems;
ArrayAdapter<String> vehicleTypeAdapterItems;
TextInputLayout nic, password, vehicleNumber;
Button signUp;

    @Override
    public View onCreateView(
            LayoutInflater inflater, ViewGroup container,
            Bundle savedInstanceState
    ) {

        //View view =  inflater.inflate(R.layout.driver_sign_up_page, container, false);
        binding = DriverSignUpPageBinding.inflate(inflater, container, false);
        View view = binding.getRoot();

        fuelType = view.findViewById(R.id.fuel_type);
        vehicleType = view.findViewById(R.id.vehicle_type);
        nic = view.findViewById(R.id.nic_field);
        vehicleNumber = view.findViewById(R.id.vehicle_number_field);
        password = view.findViewById(R.id.password_field);
        signUp = view.findViewById(R.id.driver_sign_up_button);


        fuelTypeAdapterItems = new ArrayAdapter<String>(getActivity(),R.layout.fuel_type_list,fuelTypes);
        vehicleTypeAdapterItems = new ArrayAdapter<String>(getActivity(),R.layout.vehicle_type_list,vehicleTypes);

        fuelType.setAdapter(fuelTypeAdapterItems);
        vehicleType.setAdapter(vehicleTypeAdapterItems);

        return view;

    }

    public void onViewCreated(@NonNull View view, Bundle savedInstanceState) {
        super.onViewCreated(view, savedInstanceState);

        binding.driverSignUpButton.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                NavHostFragment.findNavController(DriverSignUpActivity.this)
                        .navigate(R.id.driver_sign_up_to_FirstFragment);

                String nicNumber =  nic.getEditText().getText().toString();
                String vNumber =  vehicleNumber.getEditText().getText().toString();
                String fType =  fuelType.getText().toString();
                String vType =  vehicleType.getText().toString();
                String pass =  password.getEditText().getText().toString();


                registerDriver(nicNumber, vNumber, vType, fType, pass);
                //getFuelTypes();

            }

        });
    }

    private void registerDriver(String nic, String vNumber,String vType, String fType, String password){
        RetrofitClient retrofitClient = RetrofitClient.getInstance();
        Driver driver = new Driver(nic, password, vNumber, fType, vType);
        HttpsTrustManager.allowAllSSL();

        JSONObject newDriver = new JSONObject();  //if needed
        try {
            newDriver.put("nic", nic);
            newDriver.put("vehicleNumber", vNumber);
            newDriver.put("fuelType", fType);
            newDriver.put("vehicleType", vType);
            newDriver.put("password", password);

        } catch (JSONException e) {
            e.printStackTrace();
        }

        Call<Driver> call = retrofitClient.getMyApi().registerDriver(driver);
        Log.i("payload", driver.getNic());

        call.enqueue(new Callback<Driver>() {
            @Override
            public void onResponse(Call<Driver> call, Response<Driver> response) {

                Driver responseFromAPI = response.body();
                String responseString = "Response Code : " + response.code();
                Log.i("response", responseString);

            }

            @Override
            public void onFailure(Call<Driver> call, Throwable t) {

                Log.i("response", String.valueOf(t));

            }
        });

    }

    private void getFuelTypes(){
        RetrofitClient retrofitClient = RetrofitClient.getInstance();
        HttpsTrustManager.allowAllSSL();
        Call<List<FuelType>> call = retrofitClient.getMyApi().getFuelTypes();

        call.enqueue(new Callback<List<FuelType>>() {
            @Override
            public void onResponse(Call<List<FuelType>> call, Response<List<FuelType>> response) {

                String responseString = "Response Code : " + response.code();
                Log.i("response", responseString);

            }

            @Override
            public void onFailure(Call<List<FuelType>> call, Throwable t) {

                Log.i("response", String.valueOf(t));

            }
        });

    }

@Override
    public void onDestroyView() {
        super.onDestroyView();
        binding = null;
    }

}