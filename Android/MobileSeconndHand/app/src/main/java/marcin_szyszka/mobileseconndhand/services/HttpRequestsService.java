package marcin_szyszka.mobileseconndhand.services;

import android.content.Context;

import com.loopj.android.http.AsyncHttpClient;
import com.loopj.android.http.AsyncHttpResponseHandler;
import com.loopj.android.http.RequestParams;

import java.util.Map;

import marcin_szyszka.mobileseconndhand.AppConstant;

/**
 * Created by marcianno on 2016-02-12.
 */
public class HttpRequestsService {
    private static final String WEB_API_BASE_URL = AppConstant.WEB_API_URL;
    private static AsyncHttpClient client = new AsyncHttpClient();

    public static void get(String url, RequestParams params, Map<String, String> headers, AsyncHttpResponseHandler responseHandler) {
        if (headers != null){
            AddHeadersToClient(headers);
        }
        client.get(getAbsoluteUrl(url), params, responseHandler);
    }

    public static void post(String url, RequestParams params, Map<String, String> headers, AsyncHttpResponseHandler responseHandler) {
        if (headers != null){
            AddHeadersToClient(headers);
        }
        client.post(getAbsoluteUrl(url), params, responseHandler);
    }

    public static void getByUrl(String url, RequestParams params, Map<String, String> headers, AsyncHttpResponseHandler responseHandler) {
        if (headers != null){
            AddHeadersToClient(headers);
        }
        client.get(url, params, responseHandler);
    }

    public static void postByUrl(String url, RequestParams params, AsyncHttpResponseHandler responseHandler) {
        client.post(url, params, responseHandler);
    }

    private static String getAbsoluteUrl(String relativeUrl) {
        return WEB_API_BASE_URL + relativeUrl;
    }

    private static void AddHeadersToClient(Map<String, String> headers) {
        for (Map.Entry<String, String> entry:
                headers.entrySet()) {
            String key = entry.getKey();
            String value = entry.getValue();
            client.addHeader(key, value);
        }
    }

}
