package com.mobile.application.payin.common.utilities;

import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentManager;
import android.support.v4.app.FragmentStatePagerAdapter;

import com.mobile.application.payin.views.FragmentPagos;
import com.mobile.application.payin.views.FragmentServicios;
import com.mobile.application.payin.views.FragmentTarjetas;

public class ViewPagerAdapter extends FragmentStatePagerAdapter {
    private FragmentServicios fs;
    private FragmentPagos fp;
    private FragmentTarjetas ft;

    private CharSequence Titles[];
    private int NumbOfTabs;

    public ViewPagerAdapter(FragmentManager fm, CharSequence mTitles[], int mNumbOfTabsumb) {
        super(fm);

        this.Titles = mTitles;
        this.NumbOfTabs = mNumbOfTabsumb;

        fs = new FragmentServicios();
        fp = new FragmentPagos();
        ft = new FragmentTarjetas();
    }

    @Override
    public Fragment getItem(int position) {

        if (NumbOfTabs == 2) {
            switch (position) {
                case 0:
                    return fp;
                case 1:
                    return ft;
            }
        } else {
            switch (position) {
                case 0:
                    return fs;
                case 1:
                    return fp;
                case 2:
                    return ft;
            }
        }

        return null;
    }

    @Override
    public CharSequence getPageTitle(int position) {
        return Titles[position];
    }

    @Override
    public int getCount() {
        return NumbOfTabs;
    }
}
